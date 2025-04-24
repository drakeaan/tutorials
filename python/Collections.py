# Dictionaries, Arrays, Lists, Tuples etc...

# Tuple
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

# similar to C#. for key, value in ...can items() be inferred? or is it required?
# apparently not. dictionary is the object and the enumeration is not inferred.
for k, v in dictionary.items():
    print(f"Key: {k} | Value: {v}")

# type in python is just <class, 'list'>
# ensure type safety or parsing 
listthing = ["a", "b", "c"]
listthing2 = ["a", 2, True]