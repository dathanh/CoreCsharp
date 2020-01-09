const program = require('commander');
const fs = require('fs');
const readline = require('readline');
const dir = require('../dir');
const allEntity = require('../Core/AllEntities');
const Handlebars = require('handlebars');

String.prototype.capitalize = function() {
    return this.replace(/(?:^|\s)\S/g, function(a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('import')
    .action(() => {
        fs.writeFileSync(genDir + '/dataTempControllerJs.js', '');
        allEntity.forEach(entityName => {
            const data = fs.readFileSync(genDir + 'Template/Main.html');
            const template = Handlebars.compile(data.toString());
            var contents = template({
                entityName: entityName.capitalize(),
                entityVar: entityName.toLowerCase(),
            });
            fs.appendFileSync(genDir + '/dataTempControllerJs.js', '\n' + contents + "\n");
        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.Main)
        });
        // event is emitted after each line
        let isImport = true;
        rl.on('line', function(line) {
            const mainStart = '//========Import Controller Start========//';
            const mainEnd = '//========Import Controller End========//';
            if (line.includes(mainStart)) {
                fs.appendFileSync(genDir + '/maintemp.js', line.toString() + "\n");
                isImport = false;
            }
            if (line.includes(mainEnd)) {
                var dataImport = fs.readFileSync(genDir + '/dataTempControllerJs.js', '');
                fs.appendFileSync(genDir + '/maintemp.js', dataImport);
                fs.appendFileSync(genDir + '/maintemp.js', line.toString() + "\n");
                isImport = true;
            } else if (isImport) {
                fs.appendFileSync(genDir + '/maintemp.js', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/maintemp.js', dir.Main);
            fs.unlinkSync(genDir + '/maintemp.js');
            fs.unlinkSync(genDir + '/dataTempControllerJs.js');
            console.log(`import main ${entityName} complete !!!`);
        }, 1000);
    });
program.parse(process.argv);