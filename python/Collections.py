# Dictionaries, Arrays, Lists, Tuples etc...

# Tuple
from typing import List


[(1,2), (3,4)]

# ugh... I don't like this... it means it's basically Dictionary<object, object>()
# e.g. <class, 'dict'>
# so does that mean ensuring type safety?? it seems the type in python is just 'dict'.
dictionary = {
    "brand": "a",
    "foo": "bar",
    "int": 2025
}

print(len(dictionary))

# similar to C#. for key, value in... can items() be inferred? or is it required?
# apparently not. dictionary is the object and the enumeration is not inferred.
for k, v in dictionary.items():
    print(f"Key: {k} | Value: {v}")

# type in python is just <class, 'list'>
# ensure type safety or parsing 
listthing = ["a", "b", "c"]
listthing2 = ["a", 2, True]

class Solution:
    def plusOne(self, digits: List[int]) -> List[int]:
        ## currently fails on [9,9] - to be investigated
        lastDigit = digits[-1]
        returnList = []

        lastDigit = lastDigit + 1

        for i in range(len(digits) -1):
            returnList.append(digits[i])

        if len(str(lastDigit)) > 1:
            for i in str(lastDigit):
                returnList.append(int(i))
        else:
            returnList.append(lastDigit)
        return returnList
        