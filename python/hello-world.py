import sys

sys.stdout.write("hello world\n")
sys.stdout.write('hello world\n')

# print consumes sys.stdout. So an easier way. 
# also includes line break (\n) by default whereas with `stdout` it needs to be add explicitly.
# TODO: determine difference between CRLF and LF on \n and \r

# probably a bit slower though should be measured in nanoseconds.
# TODO: determine if there is a difference between ' and " in python.
print("hello world")
print('hello world')