import System
#import sys
#sys.modules["System"]
#System.Collections
#System.Environment

#from System.DateTime import Now
#a_second_ago1 = Now
#import time
#time.sleep(5)
#from System.DateTime import Now
#a_second_ago2 = Now

#a = a
#b = b
#num1 = 0
#num2 = 0

def sum(num1, num2):
    sum = num1 + num2
    return sum

def dif(num1, num2):
    dif = num1 - num2
    return dif

def multiply(num1, num2):
    mult = num1 * num2
    return mult

def div(num1, num2):
    div = num1 / num2
    return div

 #sumResult = sum(a, b)
 #multResult = multiply(a, b)

def universalFunction(num1, num2, sign):
    if sign == "+":
        return sum(num1, num2)
    elif sign == "-":
        return dif(num1, num2)
    elif sign == "*":
        return multiply(num1, num2)
    elif sign == "/":
        return div(num1, num2)