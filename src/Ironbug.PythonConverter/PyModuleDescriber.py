import os, sys
import inspect
import json

#sys.path.append("C:\\Program Files\\McNeel\\Rhinoceros 5.0\\Plug-ins\\IronPython\\Lib");
#sys.path.append("C:\\Users\\mpeng\\AppData\\Roaming\\McNeel\\Rhinoceros\\5.0\\scripts")
#from radiance.command import *
from honeybee.radiance.command.raTiff import RaTiff 

#from honeybee.room import Room

#Modified based on:
#http://code.activestate.com/recipes/553262-list-classes-methods-and-functions-in-a-module/

class PyModuleDescriber(object):

	def __init__(self):
	    return "sfdsfasd"

	def checkIfOverride(self,obj,baseClasses):
		for base in baseClasses:
			if(obj.__name__ in dir(base)):
				return True
			else:
				return False

	def checkIfReturn(self,method):
		code = inspect.getsourcelines(method)[0];
		if("return" in code[-1]):
			return True
		else:
			return False

	def describe_func(self,obj, method=False, isOverrideMethod = False):
		""" Describe the function object passed as argument.
		If this is a method object, the second argument will
		be passed as True """
		type = "Method"
		isOverride = False
		if method:
			type = "Method"
		else:
			type = "Function"
   
		objName = obj.__name__
		if objName == "__init__":
			type = "Constructor"
   
		if isOverrideMethod:
			isOverride = True
		else:
			isOverride = False
   
		ifReturn = self.checkIfReturn(obj)
   
		try:
			arginfo = inspect.getargspec(obj)
		except TypeError:
			return
   
		args = arginfo[0]
		argsvar = arginfo[1]
   
		if args:
			if arginfo[3]:
				dl = len(arginfo[3])
				al = len(args)
				defargs = args[al-dl:al]
				defaultArgs = zip(defargs, arginfo[3])

		funcDict = {"Type": type,"IsOverride":isOverride,"IfReturn":ifReturn, "Name": objName, "Arguments": args, "DefaultArgs": defaultArgs}
		return funcDict

	def describe_klass(self,obj):
	   """ Describe the class object passed as argument,
	   including its methods """
	   wi('+Class: %s' % obj.__name__)
	   for name in obj.__dict__:
		   item = getattr(obj, name)
		   if inspect.ismethod(item):
			   count += 1;describe_func(item, True)
	   if count==0:
		   wi('(No members)')
	   print 

	def describe(self,module):
		""" Describe the module object passed as argument
		including its classes and functions """
		baseClasses = module.__bases__
		baseNames = []
		for i in baseClasses:
			baseNames.append(i.__name__)
		count = 0
		methods = []
		properties = []
		for name in module.__dict__:
			obj = getattr(module, name)
       
       
			if inspect.isclass(obj):
				count += 1; 
				self.describe_klass(obj)
			elif (inspect.ismethod(obj)):
				count +=1 ; 
				isOverride = self.checkIfOverride(obj,baseClasses)
				methods.append(self.describe_func(obj, True, isOverride))
          
			elif inspect.isfunction(obj):
				count +=1 ; 
				isOverride = self.checkIfOverride(obj,baseClasses)
				methods.append(self.describe_func(obj, False, isOverride))
          
			if isinstance(obj, property):
				properties.append(name)
           
       
		if count==0:
			wi('(No members)')
      
		moduleDict = {"Bases": baseNames,"Properties":properties,"Methods":methods, "Name": module.__name__}
		
		return json.dumps(moduleDict)


#module = RaTiff
jsonobj= PyModuleDescriber().describe(RaTiff)