# Arrays
apps=(a b c)

# Associative array... basically a dictionary with Key|Value pairing
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