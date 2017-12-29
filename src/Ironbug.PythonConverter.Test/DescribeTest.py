import imp
import sys
file = r"C:\Users\Mingbo\Documents\GitHub\Ironbug\src\Ironbug.PythonConverter\PyModuleDescriber.py";
foo = imp.load_source('PyModuleDescriber', file)
dec = foo.PyModuleDescriber()


#from honeybee.radiance.command.objview import Objview;
import honeybee.radiance.command.epw2wea
import honeybee.radiance.recipe.parameters
import honeybee.radiance.material.plastic

modName = 'honeybee.radiance.command.epw2wea'
#modName = 'honeybee.radiance.recipe.parameters'
modName = 'honeybee.radiance.material.plastic'

mod = sys.modules[modName]

print dec.describe(mod);


####################################

import sys
a = []
for module in sys.modules.keys():
    if module.startswith( 'honeybee.radiance.command' ):
        a.append( module)



####################################



import importlib
import pkgutil
import pyclbr



#Modified based on:
#https://stackoverflow.com/questions/3365740/how-to-import-all-submodules
#TODO: currently cannot import the models without class
def import_submodules(package, recursive=True):
    
    """ Import all submodules of a module, recursively, including subpackages
    :param package: package (name or actual module)
    :type package: str | module
    :rtype: dict[str, types.ModuleType]
    """
    if isinstance(package, str):
        package = importlib.import_module(package)
        
    results = {}
    modules = {}
    modules['Errors'] = []
    
    for loader, name, is_pkg in pkgutil.walk_packages(package.__path__):
        full_name = package.__name__ + '.' + name
        
        try:
            moduleObj = importlib.import_module(full_name)
            results[full_name] = moduleObj
            #print full_name
            
            
            #module_classList = pyclbr.readmodule(full_name).values()
            
            ##pyclbr.readmodule doesn't read any functions, but class with methods
            module_info = pyclbr.readmodule(full_name)
            
            for className, data in sorted(module_info.items(), key=lambda x:x[1].lineno):
                full_pre_name = package.__name__ + '.' +os.path.splitext(os.path.basename(data.file))[0]
                if not modules.has_key(full_pre_name):
                    modules[full_pre_name] = []
                    
                if className not in modules[full_pre_name]:
                    modules[full_pre_name].append(className)
                    
            if recursive and is_pkg:
                results.update(import_submodules(full_name))
                
        except ImportError as e:
            msg = full_name+ ': '+str(e);
            results["ERROR: "+full_name] = msg
            modules['Errors'].append(msg)
            #print msg
    return results


import honeybee
obj = import_submodules(honeybee)
print len(obj.keys())

a =  json.dumps(obj)