import imp
foo = imp.load_source('PyModuleDescriber', 'C:\\Users\\mpeng\\Documents\\GitHub\\Ironbug\\src\\Ironbug.PythonConverter\\PyModuleDescriber.py')

from honeybee.radiance.command.objview import Objview;
dec = PyModuleDescriber()
print dec.describe(Objview);