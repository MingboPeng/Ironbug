import sys
import os
import inspect
import json

import importlib
import pkgutil
import pyclbr

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
			
	def checkValuableType(self,data):
		typeName = type(data).__name__
		#print typeName
		if(typeName == "int"):return "int"
		elif(typeName == "float"):return "float"
		elif(typeName == "bool"):return "bool"
		elif(typeName == "str"):return "string"
		elif(typeName == "dict"):return "dictionary"
		elif(typeName == "list"):return "list"
		elif(typeName == "long"):return "long"
		elif(typeName == "tuple"):return "tuple"
		else:return "object"
	
	def creatValuable(self,name, data):
		objType = self.checkValuableType(data)
		if (objType == "tuple"):
			data = list(data)
			objType = "list"
		valuableObj = {"Name":name,"Type":objType,"DefaultValue":data}
		return valuableObj

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
		defaultArgs = []
		arguments = []

		if args: #### this including "self" in methods
			if arginfo[3]:
				dl = len(arginfo[3])
				al = len(args)
				defargs = args[al-dl:al] #### this is clean arguments
				for name, data in zip(defargs, arginfo[3]):
					arguments.append(self.creatValuable(name, data))
					
				#defaultArgs = zip(defargs, arginfo[3])
		print arguments

		funcDict = {"Type": type,"IsOverride":isOverride,"IfReturn":ifReturn, "Name": objName, "Arguments": arguments}
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



	def import_submodules(self, package, recursive=True):
		
		""" Import all submodules of a module, recursively, including subpackages
		:param package: package (name or actual module)
		:type package: str | module
		:rtype: dict[str, types.ModuleType] """

		if isinstance(package, str):
			package = importlib.import_module(package);

		results = {}

		for loader, name, is_pkg in pkgutil.walk_packages(package.__path__):
			full_name = package.__name__ + '.' + name

			try:
				moduleObj = importlib.import_module(full_name)
				results[full_name] = moduleObj
				if recursive and is_pkg:
					results.update(self.import_submodules(full_name))
			except ImportError as e:
				msg = full_name+ ': '+str(e);
				results["ERROR: "+full_name] = msg
				
		return results

	def describeAll(self, package):
		mods = self.import_submodules(package)
		modsDes = []
		for i in mods:
			if not i.startswith('honeybee.radiance.recipe.parameters'): #<<<<<<<<=============== only for test
				continue;
			mod = mods[i];
			if i.startswith('ERROR'):
				print (mod)
			else:
				#print i
				modsDes.append(self.describe(mod))
		return modsDes;
		
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
				mod_valuables.append(self.creatValuable(i, obj))
				
		moduleDict = {"Name":module.__name__ ,"Classes":mod_classes, "Functions":mod_functions, "Valuables":mod_valuables};
		#print module
		#return json.dumps(moduleDict);
		return moduleDict;

#import honeybee
#from honeybee.radiance.command.raTiff import RaTiff 
#module = RaTiff
#obj = PyModuleDescriber().describeAll(honeybee)
#a = obj