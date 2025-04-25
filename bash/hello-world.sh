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