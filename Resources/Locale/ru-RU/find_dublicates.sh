rg --glob '*.ftl' -n '^[^[:space:].][^=]*=' \
| awk -F':' '
{
    line=$0

    split($0, parts, "=")
    key=parts[1]

    sub(/^[^:]*:[0-9]+:/, "", key)
    gsub(/^[ \t]+|[ \t]+$/, "", key)

    count[key]++
    entries[key]=entries[key] "\n" line
}
END {
    for (k in count)
        if (count[k] > 1)
            print "DUPLICATE: " k entries[k] "\n"
}'

read