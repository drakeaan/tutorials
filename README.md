# General tutorials for me to learn and practice Python and shell scripting.

## Main takeways so far:
 - Use `pip install -r requirements.txt` to install all dependencies listed in the requirements file.
 - Use `pip freeze > requirements.txt` to create a requirements file with all the installed packages and their versions.
 - Use `pip install --upgrade <package>` to upgrade a specific package.
 - Use `pip uninstall <package>` to uninstall a specific package.
 - Use `pip list` to list all installed packages and their versions.

### Python:
Python is a high-level, interpreted programming language known for its readability and versatility. It is widely used in web development, data analysis, artificial intelligence, scientific computing, and more.
Python's syntax is designed to be easy to read and write, making it a popular choice for beginners and experienced developers alike. It supports multiple programming paradigms, including procedural, object-oriented, and functional programming. Python has a large standard library and a vibrant ecosystem of third-party packages, making it suitable for a wide range of applications.

There seems to be a lot of overlap with C++ especially in the syntax. I think I will be able to pick it up quickly.

Numerical values seems to prefer the C++ style of 0.L for floats/doubles and 0 for integers. I think this is a good idea as it makes it clear what type of number you are dealing with. I will try to stick to this style in my code even in C# to make it easier to switch between languages.

e.g.
num = 123.4567
print(f"Number: {num:.2f}") # Keep 2 decimals
print(f"Number: {num:10.2f}") # Keep 2 decimals, total length is 10
print(f"Number: {num:<10.2f}") # Keep 2 decimals, total length is 10, left align
print(f"Number: {num:>10.2f}") # Keep 2 decimals, total length is 10, right align
print(f"Number: {num:^10.2f}") # Keep 2 decimals, total length is 10, center align


### Shell scripting:
Shell scripting is a powerful way to automate tasks and manage system operations in Unix-like operating systems. It allows users to write scripts that can execute a series of commands, making it easier to perform repetitive tasks, manage files, and control system processes. Shell scripts are typically written in a shell programming language, such as Bash, and can be executed directly from the command line or saved as executable files.

#### AWK
AWK is a powerful text processing and pattern scanning language. It is commonly used for data extraction and reporting, making it ideal for tasks such as parsing log files, manipulating text files, and generating reports. AWK operates on a line-by-line basis, allowing users to define patterns and actions to be performed on matching lines. Its syntax is concise and expressive, making it a popular choice for quick data manipulation tasks in shell scripting.

**KEY CONCEPTS**<br/>
`$` (Dollar sign): <br/>
&nbsp;&nbsp;&nbsp;&nbsp;Used to refer to fields within a record. $ followed by a number (e.g., $1, $2) refers to the first, second, etc., field. <br/>
`FS`:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;The field separator. By default, it's whitespace, but you can change it using the -F option (e.g., awk -F: '{print $1}' /etc/passwd to use a colon as the field separator). <br/>
`NR`:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;The record number (line number). <br/>
`END` block:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;Actions within the END block are executed after processing all lines in the file.<br/>
Regular Expressions:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;Used for pattern matching. They can be used within the awk command to specify complex patterns. <br/>

#### SED
SED (Stream Editor) is a powerful text processing tool used for parsing and transforming text in a pipeline. It allows users to perform basic text transformations on an input stream (a file or input from a pipeline) using a simple and compact syntax. SED is particularly useful for tasks such as search and replace, text substitution, and line manipulation. It operates on a line-by-line basis, making it efficient for processing large files or streams of data. SED is often used in shell scripting to automate text processing tasks and can be combined with other command-line tools for more complex workflows.

#### GREP
GREP (Global Regular Expression Print) is a command-line utility used for searching plain-text data for lines that match a specified pattern. It is widely used in Unix-like operating systems and is a powerful tool for text processing and data extraction. GREP supports regular expressions, allowing users to define complex search patterns. It can be used to search files, filter output from other commands, and perform text analysis tasks. GREP is often combined with other command-line tools in shell scripting to create efficient workflows for data manipulation and analysis.

#### FIND
FIND is a command-line utility used to search for files and directories in a directory hierarchy based on various criteria. It is commonly used in Unix-like operating systems and provides a powerful way to locate files based on attributes such as name, size, modification time, and permissions. FIND can also execute commands on the found files, making it a versatile tool for file management and automation tasks. Its syntax allows for complex search patterns and options, enabling users to perform precise searches in large file systems. FIND is often used in shell scripting to automate file-related tasks and streamline workflows.

#### XARGS
XARGS is a command-line utility used to build and execute command lines from standard input. It is commonly used in Unix-like operating systems and is particularly useful for processing large amounts of data or output from other commands. XARGS takes input from standard input (stdin) and converts it into arguments for a specified command, allowing users to efficiently pass data between commands in a pipeline. It can handle multiple arguments and supports various options for controlling how the input is processed. XARGS is often used in shell scripting to automate tasks that involve multiple files or data streams.