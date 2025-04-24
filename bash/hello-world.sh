echo "hello world"

# %s = string
# %d = number...
# go back to old school win command line stuff... grown too used to pscore
printf "%s\n" "hello world"
printf "%d\n" 123

# cat but feed back into stdout... <<
cat << EOF
Hello world,
This is a multiline string.
EOF

# AWK is a powerful text processing tool commonly used in bash scripting. It operates on a line-by-line basis, interpreting each line as a record and dividing it into fields. The basic syntax of an AWK command is:
awk 'pattern { action }' file
# e.g.
awk -F"," '{ print $2 }' data.csv # Prints the second field of each line in data.csv, using comma as a delimiter

# SED --> stream editor. refer to gh workflow actions...
###
    # Option    Description
    # -i	    Edit the file in place without printing to the console (overwrite the file).
    # -n	    Suppress automatic printing of lines.
    # -e	    Allows multiple commands to be executed.
    # -f	    Reads sed commands from a file instead of the command line.
    # -r	    Enables extended regular expressions.
###
# e.g. replace the current tag (i.e 123) for the helm chart with "456"
sed -i 's/tag\:.*/tag: "456"/' foo.yaml

# Arrays
apps=(a b c)

# Associative array...
declare -A environments
environments[a]='foo'
environments[b]='bar'

###
    # ${array[@]} expands to all elements of the array, with each element treated as a separate word. 
    # This is commonly used when iterating over or processing all elements.
###

# Append to array.
# REMEMBER: no spaces between var+=array
apps+=( d e f g)
environments[c]='something else'

# Get the count...
echo "Total apps: ${#apps[@]}"
echo "Total environments: ${#environments[@]}"

# get the keys and value
# Keys --> use the !
echo "Keys: ${!environments[@]}"
# Values --> without the !
echo "Values: ${environments[@]}"

#variables...
# declare a var...
HTTP_CODE=200
# access the var... note the $ when accessing
echo $HTTP_CODE

# looping through arrays. REMEMBER: the @ expands all elements within the array
# in the first FOR, we're iterating over the keys.Note the !.
for env in "${!environments[@]}"; do
    # note the $ before env as when declaring the var, the $ is missing but when accessing, the $ is required.
    echo "${env} => ${environments[$env]}"
    for app in "${apps[@]}"; do
        if test -f foo.yaml; then
            sed -i 's/tag\:.*/tag: "123"/' foo.yaml
        fi
    done
done