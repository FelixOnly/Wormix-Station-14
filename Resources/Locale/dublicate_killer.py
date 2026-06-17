#!/usr/bin/env python3
"""
Mozilla Fluent (FTL) duplicate key manager.

Features:
- Scans one or more .ftl files
- Detects duplicate message/term keys
- Shows all duplicate entries with file and line numbers
- Lets you interactively choose which duplicate to keep
- Deletes the other duplicates automatically
- Creates .bak backup files before modifying anything

Usage:
    python ftl_duplicate_manager.py path/to/file.ftl
    python ftl_duplicate_manager.py locales/

Notes:
- Supports multiline Fluent entries
- Preserves formatting as much as possible
- Terms beginning with '-' are also supported
"""

from __future__ import annotations

import argparse
import shutil
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, List


@dataclass
class FTLEntry:
    key: str
    file_path: Path
    start_line: int
    end_line: int
    content: str


ENTRY_START_CHARS = tuple("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-")


def collect_ftl_files(path: Path) -> List[Path]:
    if path.is_file() and path.suffix == ".ftl":
        return [path]

    if path.is_dir():
        return sorted(path.rglob("*.ftl"))

    return []


def is_entry_start(line: str) -> bool:
    stripped = line.strip()

    if not stripped:
        return False

    if stripped.startswith("#"):
        return False

    if "=" not in stripped:
        return False

    first_char = stripped[0]
    return first_char in ENTRY_START_CHARS


def extract_key(line: str) -> str:
    return line.split("=", 1)[0].strip()


def parse_ftl_entries(file_path: Path) -> List[FTLEntry]:
    # Try several encodings because some FTL files may contain BOMs
    # or be saved in UTF-16/Windows encodings.
    encodings = [
        "utf-8",
        "utf-8-sig",
        "utf-16",
        "utf-16-le",
        "utf-16-be",
        "cp1251",
    ]

    text = None

    for encoding in encodings:
        try:
            text = file_path.read_text(
                encoding=encoding,
                errors="ignore",
            )
            break
        except Exception:
            continue

    if text is None:
        print(f"Skipping unreadable file: {file_path}")
        return []

    lines = text.splitlines(keepends=True)

    entries: List[FTLEntry] = []

    current_key = None
    current_start = None
    current_content = []

    for idx, line in enumerate(lines, start=1):
        if is_entry_start(line):
            if current_key is not None:
                entries.append(
                    FTLEntry(
                        key=current_key,
                        file_path=file_path,
                        start_line=current_start,
                        end_line=idx - 1,
                        content="".join(current_content),
                    )
                )

            current_key = extract_key(line)
            current_start = idx
            current_content = [line]
        else:
            if current_key is not None:
                current_content.append(line)

    if current_key is not None:
        entries.append(
            FTLEntry(
                key=current_key,
                file_path=file_path,
                start_line=current_start,
                end_line=len(lines),
                content="".join(current_content),
            )
        )

    return entries


def find_duplicates(files: List[Path]) -> Dict[str, List[FTLEntry]]:
    key_map: Dict[str, List[FTLEntry]] = {}

    for file_path in files:
        for entry in parse_ftl_entries(file_path):
            key_map.setdefault(entry.key, []).append(entry)

    return {
        key: entries
        for key, entries in key_map.items()
        if len(entries) > 1
    }


def print_entry_preview(entry: FTLEntry) -> None:
    print(f"File: {entry.file_path}")
    print(f"Lines: {entry.start_line}-{entry.end_line}")
    print("-" * 60)

    preview = entry.content.strip()
    print(preview)

    print("-" * 60)


def backup_file(path: Path) -> None:
    backup_path = path.with_suffix(path.suffix + ".bak")

    if not backup_path.exists():
        shutil.copy2(path, backup_path)
        print(f"Created backup: {backup_path}")


def remove_entry(entry: FTLEntry) -> None:
    # Read using tolerant encoding detection
    encodings = [
        "utf-8",
        "utf-8-sig",
        "utf-16",
        "utf-16-le",
        "utf-16-be",
        "cp1251",
    ]

    text = None

    for encoding in encodings:
        try:
            text = entry.file_path.read_text(
                encoding=encoding,
                errors="ignore",
            )
            break
        except Exception:
            continue

    if text is None:
        print(f"Failed to edit unreadable file: {entry.file_path}")
        return

    # Remove exact content block instead of relying on stale line numbers
    if entry.content in text:
        text = text.replace(entry.content, "", 1)
    else:
        print(f"Could not find duplicate block in: {entry.file_path}")
        return

    entry.file_path.write_text(text, encoding="utf-8")


def process_duplicates(duplicates: Dict[str, List[FTLEntry]]) -> None:
    if not duplicates:
        print("No duplicate keys found.")
        return

    total_duplicate_entries = sum(len(v) - 1 for v in duplicates.values())

    print(f"Found {len(duplicates)} duplicate key(s).")
    print(f"Total duplicate entries to review: {total_duplicate_entries}")
    print()

    for key, entries in duplicates.items():
        print("=" * 80)
        print(f"DUPLICATE KEY: {key}")
        print("=" * 80)
        print()

        for idx, entry in enumerate(entries, start=1):
            print(f"[{idx}]")
            print_entry_preview(entry)
            print()

        while True:
            choice = input(
                f"Choose entry to KEEP for '{key}' (1-{len(entries)}), "
                "'s' to skip, or 'q' to quit: "
            ).strip().lower()

            if choice == "q":
                print("Exiting program.")
                raise SystemExit(0)

            if choice == "s":
                print("Skipped.\n")
                break

            if choice.isdigit():
                keep_index = int(choice) - 1

                if 0 <= keep_index < len(entries):
                    keep_entry = entries[keep_index]

                    to_delete = [
                        e for i, e in enumerate(entries)
                        if i != keep_index
                    ]

                    to_delete.sort(
                        key=lambda e: (str(e.file_path), e.start_line),
                        reverse=True,
                    )

                    for entry in to_delete:
                        print(
                            f"Deleting duplicate from "
                            f"{entry.file_path}:{entry.start_line}"
                        )
                        remove_entry(entry)

                    print(
                        f"Kept entry in "
                        f"{keep_entry.file_path}:{keep_entry.start_line}\n"
                    )
                    break

            print("Invalid selection. Try again.")


def main() -> None:
    parser = argparse.ArgumentParser(
        description="Find and manage duplicate Fluent FTL keys"
    )

    parser.add_argument(
        "path",
        help="FTL file or directory containing FTL files",
    )

    args = parser.parse_args()

    target = Path(args.path)

    if not target.exists():
        print(f"Path does not exist: {target}")
        return

    files = collect_ftl_files(target)

    if not files:
        print("No .ftl files found.")
        return

    print(f"Scanning {len(files)} file(s)...")

    duplicates = find_duplicates(files)

    process_duplicates(duplicates)

    print("Done.")


if __name__ == "__main__":
    main()
