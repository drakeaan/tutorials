[![Board Status](https://dev.azure.com/drakeaan/89b7138a-4d94-4d3c-8c15-8b50816ee5aa/00f2fa8a-cecb-48e4-8023-c2f9f8ba9a5d/_apis/work/boardbadge/2a645c18-072d-4a8b-95a4-f8a03f9ab2b3)](https://dev.azure.com/drakeaan/89b7138a-4d94-4d3c-8c15-8b50816ee5aa/_boards/board/t/00f2fa8a-cecb-48e4-8023-c2f9f8ba9a5d/Microsoft.RequirementCategory)
# General tutorials for me to learn and practice Python and shell scripting.

## Main takeways so far:
TBD

## Python:
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

 - Use `pip install -r requirements.txt` to install all dependencies listed in the requirements file.
 - Use `pip freeze > requirements.txt` to create a requirements file with all the installed packages and their versions.
 - Use `pip install --upgrade <package>` to upgrade a specific package.
 - Use `pip uninstall <package>` to uninstall a specific package.
 - Use `pip list` to list all installed packages and their versions.

## Shell scripting:
Shell scripting is a powerful way to automate tasks and manage system operations in Unix-like operating systems. It allows users to write scripts that can execute a series of commands, making it easier to perform repetitive tasks, manage files, and control system processes. Shell scripts are typically written in a shell programming language, such as Bash, and can be executed directly from the command line or saved as executable files.

### AWK
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

### SED
SED (Stream Editor) is a powerful text processing tool used for parsing and transforming text in a pipeline. It allows users to perform basic text transformations on an input stream (a file or input from a pipeline) using a simple and compact syntax. SED is particularly useful for tasks such as search and replace, text substitution, and line manipulation. It operates on a line-by-line basis, making it efficient for processing large files or streams of data. SED is often used in shell scripting to automate text processing tasks and can be combined with other command-line tools for more complex workflows.

### GREP
GREP (Global Regular Expression Print) is a command-line utility used for searching plain-text data for lines that match a specified pattern. It is widely used in Unix-like operating systems and is a powerful tool for text processing and data extraction. GREP supports regular expressions, allowing users to define complex search patterns. It can be used to search files, filter output from other commands, and perform text analysis tasks. GREP is often combined with other command-line tools in shell scripting to create efficient workflows for data manipulation and analysis.

### FIND
FIND is a command-line utility used to search for files and directories in a directory hierarchy based on various criteria. It is commonly used in Unix-like operating systems and provides a powerful way to locate files based on attributes such as name, size, modification time, and permissions. FIND can also execute commands on the found files, making it a versatile tool for file management and automation tasks. Its syntax allows for complex search patterns and options, enabling users to perform precise searches in large file systems. FIND is often used in shell scripting to automate file-related tasks and streamline workflows.

### XARGS
XARGS is a command-line utility used to build and execute command lines from standard input. It is commonly used in Unix-like operating systems and is particularly useful for processing large amounts of data or output from other commands. XARGS takes input from standard input (stdin) and converts it into arguments for a specified command, allowing users to efficiently pass data between commands in a pipeline. It can handle multiple arguments and supports various options for controlling how the input is processed. XARGS is often used in shell scripting to automate tasks that involve multiple files or data streams.

## LINUX:
Linux is a family of open-source Unix-like operating systems based on the Linux kernel. It is widely used for servers, desktops, embedded systems, and various other computing devices. Linux is known for its stability, security, and flexibility, making it a popular choice for developers, system administrators, and users who value control over their computing environment. The Linux ecosystem includes a variety of distributions (distros), each tailored for specific use cases and user preferences. Some popular Linux distributions include Ubuntu, CentOS, Fedora, and Debian.

### Top-Level Directories:
- `/`: The root directory, the top-level directory in the filesystem hierarchy.
- `/bin`: Contains essential command-line utilities and binaries needed for system operation.
- `/boot`: Contains files required for booting the system, including the kernel and bootloader.
- `/dev`: Contains device files that represent hardware devices and system resources.
- `/etc`: Contains system configuration files and directories for system-wide settings.
- `/home`: Contains user home directories, where user-specific files and settings are stored.
- `/lib`: Contains shared libraries and kernel modules required for system operation.
- `/media`: Mount point for removable media devices (e.g., USB drives, CDs).
- `/mnt`: Temporary mount point for filesystems, often used for mounting external devices.
- `/opt`: Contains optional software packages and add-on applications.
- `/proc`: Virtual filesystem that provides information about system processes and kernel parameters.
- `/root`: The home directory for the root user (system administrator).
- `/run`: Contains runtime data for processes and system services.
- `/sbin`: Contains system binaries and administrative commands, typically used by the root user.
- `/srv`: Contains data for services provided by the system (e.g., web server data).
- `/sys`: Virtual filesystem that provides information about system devices and kernel subsystems.
- `/tmp`: Temporary files and directories, often cleared on system reboot.
- `/usr`: Contains user-related programs and data, including applications and libraries.
- `/var`: Contains variable data files, such as logs, databases, and spool files.
- `/usr/local`: Contains locally installed software and custom applications.
- `/usr/share`: Contains architecture-independent data files for applications.
- `/usr/bin`: Contains user command binaries and applications.
- `/usr/sbin`: Contains system binaries and administrative commands for system maintenance.
- `/usr/lib`: Contains shared libraries and modules for user applications.
- `/usr/include`: Contains header files for C and C++ programming.
- `/usr/src`: Contains source code for the Linux kernel and other software packages.
- `/usr/share/doc`: Contains documentation files for installed packages.
- `/usr/share/man`: Contains manual pages for command-line utilities and applications.
- `/usr/share/info`: Contains info files for documentation in the GNU Info format.
- `/usr/share/icons`: Contains icon themes for graphical user interfaces.
- `/usr/share/fonts`: Contains font files for graphical user interfaces.
- `/usr/share/applications`: Contains desktop entry files for applications in graphical environments.
- `/usr/share/pixmaps`: Contains bitmap images for applications and desktop environments.
- `/usr/share/lintian`: Contains files for the Lintian package checker, used in Debian-based systems.
- `/usr/share/locale`: Contains localization files for internationalization and localization of applications.