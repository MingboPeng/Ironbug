# import sys
# sys.path.append("C:\Users\Mingbo\Documents\GitHub\ladybug-tools\honeybee")
# import os

# # os.chdir("C:\Users\Mingbo\Documents\GitHub\ladybug-tools\honeybee")
# os.chdir("C:\Users\Mingbo\Test")

# import honeybee.radiance.command.raTiff
# from radiance.command.config import config

from testClass import Person


sam = Person("SamA")
samName = sam.myName()
print "printed from Python: "+samName

# class Calculator(object):
#     def add(self):
#         path = os.path.join("c:", "foo")
#         return path
