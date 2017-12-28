import imp
foo = imp.load_source('PyModuleDescriber', 'C:\\Users\\mpeng\\Documents\\GitHub\\Ironbug\\src\\Ironbug.PythonConverter\\PyModuleDescriber.py')

from honeybee.radiance.command.objview import Objview;
dec = foo.PyModuleDescriber()
print dec.describe(Objview);




##############################################
##get All modules in the package
#############################################
import importlib
import pkgutil


def import_submodules(package, recursive=True):
    
    """ Import all submodules of a module, recursively, including subpackages
    :param package: package (name or actual module)
    :type package: str | module
    :rtype: dict[str, types.ModuleType]
    https://stackoverflow.com/questions/3365740/how-to-import-all-submodules
    """
    if isinstance(package, str):
        package = importlib.import_module(package)
    results = {}
    for loader, name, is_pkg in pkgutil.walk_packages(package.__path__):
        full_name = package.__name__ + '.' + name
        
        try:
            
            results[full_name] = importlib.import_module(full_name)
            print full_name
            if recursive and is_pkg:
                results.update(import_submodules(full_name))
                
        except (ImportError):
            print full_name+ " _ error"
    return results

import honeybee

import_submodules(honeybee)
import sys
a = []
for module in sys.modules.keys():
    if module.startswith( 'honeybee.radiance.command' ):
        a.append( module)