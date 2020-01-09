const program = require('commander');
const fs = require('fs');
const readline = require('readline');
const dir = require('../dir');
const allEntity = require('../Core/AllEntities');

String.prototype.capitalize = function() {
    return this.replace(/(?:^|\s)\S/g, function(a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('import')
    .action(() => {
        fs.writeFileSync(genDir + '/dataTempCoreModule.cs', '');
        allEntity.forEach(entityName => {
            const repoImport = `builder.RegisterType<EntityFramework${entityName.capitalize()}Repository>().As<I${entityName.capitalize()}Repository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));`;
            fs.appendFileSync(genDir + '/dataTempCoreModule.cs', '            ' + repoImport + "\n");
        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.CoreModule)
        });
        // event is emitted after each line
        let isImport = true;
        rl.on('line', function(line) {
            const coreModuleStart = '//=====Start Import Repository=======//';
            const coreModuleEnd = '//=====End Import Repository=======//';
            if (line.includes(coreModuleStart)) {
                fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                isImport = false;
            }
            if (line.includes(coreModuleEnd)) {
                var dataImport = fs.readFileSync(genDir + '/dataTempCoreModule.cs', '');
                fs.appendFileSync(genDir + '/tmp.cs', dataImport);
                fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                isImport = true;
            } else if (isImport) {
                fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/tmp.cs', dir.CoreModule);
            fs.unlinkSync(genDir + '/tmp.cs');
            fs.unlinkSync(genDir + '/dataTempCoreModule.cs');
            console.log(`import Repo ${entityName} complete !!!`);
        }, 200);
    });
program.parse(process.argv);