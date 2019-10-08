const program = require('commander');
const fs = require('fs');
const readline = require('readline');
const dir = require('../dir');
const allEntity = require('../Core/AllEntities');

String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('import')
    .action(() => {
        fs.writeFileSync(genDir + '/tmp.cs', '');
        allEntity.forEach(entityName => {
            let rl = readline.createInterface({
                input: fs.createReadStream(dir.CoreModule)
            });
            // event is emitted after each line
            let isImport = true;
            rl.on('line', function (line) {
                const repoImport = `builder.RegisterType<EntityFramework${entityName.capitalize()}Repository>().As<I${entityName.capitalize()}Repository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));`;
                const condImport = line.includes('//=====Import Repository=======//');
                if (line.includes(repoImport)) {
                    isImport = false;
                }

                if (condImport && isImport) {
                    fs.appendFileSync(genDir + '/tmp.cs', '            ' + repoImport + "\n");
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                } else {
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                }
            })
            setTimeout(() => {
                fs.copyFileSync(genDir + '/tmp.cs', dir.CoreModule);
                fs.unlinkSync(genDir + '/tmp.cs');
                console.log(`import Repo ${entityName} complete !!!`);
            }, 200);

        });

    });
program.parse(process.argv);
