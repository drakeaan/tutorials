# so basically a list of 'public/internal' classes and functions
# anything not exported is handled as 'private'
__all__ = ['test_function', 'TestClass']

def test_function() -> str:
    return 'this is a test'

class TestClass:
    def __init__(self,strValue):
        self.name = strValue
    
    # TODO: figure out if the return type ( -> str) is required or inferred?
    def internal_function():
        return "this is an internal function"