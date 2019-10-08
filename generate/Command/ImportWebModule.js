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
                input: fs.createReadStream(dir.WebModule)
            });
            // event is emitted after each line
            let isImportService = true;
            let isImportBr = true;
            rl.on('line', function (line) {
                const serviceImport = `builder.RegisterType<${entityName.capitalize()}Service>().As<I${entityName.capitalize()}Service>();`;
                let brImport = ` builder.RegisterType<BusinessRuleSet<${entityName.capitalize()}>>().AsImplementedInterfaces();`  +'\n';
                brImport += `            builder.RegisterType<${entityName.capitalize()}Rule<${entityName.capitalize()}>>().AsImplementedInterfaces();`;
                const condService = line.includes('//=====Import Service=======//');
                const condBr = line.includes('//=====Import RegisterBusinessRules=======//');
                if (line.includes(serviceImport)) {
                    isImportService = false;
                }
                if (line.includes(brImport)) {
                    isImportBr = false;
                }
                if (condService || isImportBr) {
                    if (condService && isImportService) {
                        fs.appendFileSync(genDir + '/tmp.cs', '            ' + serviceImport + "\n");
                    }
                    if (condBr && isImportBr) {
                        fs.appendFileSync(genDir + '/tmp.cs', '           ' + brImport + "\n");
                    }
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                } else {
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                }
            })
            setTimeout(() => {
                fs.copyFileSync(genDir + '/tmp.cs', dir.WebModule);
                fs.unlinkSync(genDir + '/tmp.cs');
                console.log(`import Service and buseiness rule ${entityName} complete !!!`);
            }, 200);

        });

    });
program.parse(process.argv);
