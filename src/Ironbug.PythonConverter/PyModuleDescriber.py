import sys
import os
import inspect
import json

#sys.path.append("C:\\Program Files\\McNeel\\Rhinoceros 5.0\\Plug-ins\\IronPython\\Lib");
#sys.path.append("C:\\Users\\mpeng\\AppData\\Roaming\\McNeel\\Rhinoceros\\5.0\\scripts")
#from radiance.command import *
#from honeybee.radiance.command.raTiff import RaTiff 

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

	def describe_func(self,obj, isMethod=False, isOverrideMethod = False):
		""" Describe the function object passed as argument.
		If this is a method object, the second argument will
		be passed as True """
		type = "Method"
		isOverride = False
		if isMethod:
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
			else:
				defaultArgs = []

		funcDict = {"Type": type,"IsOverride":isOverride,"IfReturn":ifReturn, "Name": objName, "Arguments": args, "DefaultArgs": defaultArgs}
		return funcDict
	

	def describe_class(self,classObj):
		""" Describe the class object passed as argument,
		including its methods """
		baseNames = []
		methods = []
		properties = []
		classDict ={}

		baseClasses = classObj.__bases__;
		for i in baseClasses:
			baseclass = {"Name":i.__name__,"From":i.__module__}
			baseNames.append(baseclass);
			

		for name in classObj.__dict__:
			obj = getattr(classObj, name)

			if (inspect.ismethod(obj)):
				isOverride = self.checkIfOverride(obj,baseClasses)
				methods.append(self.describe_func(obj, isMethod=True, isOverrideMethod = isOverride))

			#elif inspect.isfunction(obj):
				#isOverride = self.checkIfOverride(obj,baseClasses)
				#methods.append(self.describe_func(obj, isMethod=False, isOverrideMethod = isOverride))
			elif isinstance(obj, property):
				properties.append(name)
		
		classDict = {"Bases": baseNames,"Properties":properties,"Methods":methods, "Name": classObj.__name__};
		return classDict;

	def describe(self,module):
		""" Describe the module object 
		including its classes and functions """
		mod_classes = []
		mod_functions = []
		mod_valuables = []


		for i in dir(module):
			if i.startswith('__'):
				continue
			obj = getattr(module,i)
			#this is already a modele, added this for filtering "Imports"
			if inspect.ismodule(obj):
				continue

			#Local class in Module
			elif inspect.isclass(obj):
				parentName = obj.__dict__['__module__']
				if parentName == module.__name__:
					classObj = self.describe_class(obj)
					
					mod_classes.append(classObj)

			#Local function in Module
			elif inspect.isfunction(obj):
				if  obj.__module__ == module.__name__:
					#print dir(obj)
					#print obj.__module__
					#print obj
					#print (i);
					mod_functions.append(self.describe_func(obj, isMethod=False, isOverrideMethod = False))

			#Local valuable in Module
			else:
				mod_valuable = {"name":i, "value":obj}
				mod_valuables.append(mod_valuable)
				
		moduleDict = {"classes":mod_classes, "functions":mod_functions, "valuables":mod_valuables};
		return json.dumps(moduleDict)


#module = RaTiff
#jsonobj= PyModuleDescriber().describe(RaTiff)