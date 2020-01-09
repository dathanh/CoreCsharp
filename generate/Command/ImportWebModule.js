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
        fs.writeFileSync(genDir + '/dataTempWebModule.cs', '');
        fs.writeFileSync(genDir + '/dataTempWebModule1.cs', '');
        allEntity.forEach(entityName => {
            const serviceImport = `builder.RegisterType<${entityName.capitalize()}Service>().As<I${entityName.capitalize()}Service>();`;
            let brImport = ` builder.RegisterType<BusinessRuleSet<${entityName.capitalize()}>>().AsImplementedInterfaces();` + '\n';
            brImport += `            builder.RegisterType<${entityName.capitalize()}Rule<${entityName.capitalize()}>>().AsImplementedInterfaces();`;
            fs.appendFileSync(genDir + '/dataTempWebModule.cs', '\n' + serviceImport + "\n");
            fs.appendFileSync(genDir + '/dataTempWebModule1.cs', '\n' + brImport + "\n");
        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.WebModule)
        });
        // event is emitted after each line
        let isImportService = true;
        let isImportBr = true;
        fs.writeFileSync(genDir + '/webModuleTemp.cs', '');
        rl.on('line', function(line) {

            const condServiceStart = line.includes('//=====Start Import Service=======//');
            const condServiceEnd = line.includes('//=====End Import Service=======//');
            const condBrStart = line.includes('//=====Start Import RegisterBusinessRules=======//');
            const condBrEnd = line.includes('//=====End Import RegisterBusinessRules=======//');
            if (condServiceStart) {
                fs.appendFileSync(genDir + '/webModuleTemp.cs', line.toString() + "\n");
                isImportService = false;
            }
            if (condBrStart) {
                fs.appendFileSync(genDir + '/webModuleTemp.cs', line.toString() + "\n");
                isImportBr = false;
            }
            if (condServiceEnd || condBrEnd) {
                if (condServiceEnd && !isImportService) {
                    var serviceImport = fs.readFileSync(genDir + '/dataTempWebModule.cs', '');
                    fs.appendFileSync(genDir + '/webModuleTemp.cs', '            ' + serviceImport + "\n");
                    isImportService = true;
                }
                if (condBrEnd && !isImportBr) {
                    var brImport = fs.readFileSync(genDir + '/dataTempWebModule1.cs', '');
                    fs.appendFileSync(genDir + '/webModuleTemp.cs', '           ' + brImport + "\n");
                    isImportBr = true;
                }
                fs.appendFileSync(genDir + '/webModuleTemp.cs', line.toString() + "\n");
            } else if (isImportService && isImportBr) {
                fs.appendFileSync(genDir + '/webModuleTemp.cs', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/webModuleTemp.cs', dir.WebModule);
            fs.unlinkSync(genDir + '/webModuleTemp.cs');
            fs.unlinkSync(genDir + '/dataTempWebModule.cs');
            fs.unlinkSync(genDir + '/dataTempWebModule1.cs');
            console.log(`import Service and buseiness rule ${entityName} complete !!!`);
        }, 200);

    });
program.parse(process.argv);