

# AWK is a powerful text processing tool commonly used in bash scripting. 
# It operates on a line-by-line basis, interpreting each line as a record and dividing it into fields. 
# The basic syntax of an AWK command is:
awk 'pattern { action }' file

# e.g.
# pattern: 
    # Can be a regular expression, a condition (e.g., NR>2), or a combination of both. 
# action:
    # Instructions to be executed when the pattern is matched. Common actions include printing, assigning variables, and performing calculations
# Prints the second field of each line in data.csv, using comma as a delimiter
awk -F"," '{ print $2 }' data.csv 

: <<'COMMENT'
KEY CONCEPTS
$ (Dollar sign):
    Used to refer to fields within a record. $ followed by a number (e.g., $1, $2) refers to the first, second, etc., field. 
FS:
    The field separator. By default, it's whitespace, but you can change it using the -F option (e.g., awk -F: '{print $1}' /etc/passwd to use a colon as the field separator). 
NR:
    The record number (line number). 
END block:
    Actions within the END block are executed after processing all lines in the file. 
Regular Expressions:
    Used for pattern matching. They can be used within the awk command to specify complex patterns. 
COMMENT

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

# Basic syntax:
grep [options] pattern [file].

: <<'COMMENT'
Options:
grep has various options to refine the search, such as:
    -i: Case-insensitive search.
    -v: Invert the match, display lines that do not match.
    -n: Display line numbers with the matching lines.
    -w: Match whole words only.
    -c: Count the number of matching lines.
COMMENT

# Examples:
grep "error" logfile.txt: Search for lines containing "error" in logfile.txt.
grep -i "hello" myfile.txt: Search for lines containing "hello" (case-insensitive) in myfile.txt.
grep -v "error" logfile.txt: Display lines in logfile.txt that do not contain "error".
