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
        fs.writeFileSync(genDir + '/dataTempRouter.cs', '');
        let navNumber = 1703;
        allEntity.forEach(entityName => {
            navNumber++;
            const data = fs.readFileSync(genDir + 'Template/Router.html');
            const template = Handlebars.compile(data.toString());
            var contents = template({
                entityName: entityName.capitalize(),
                entityVar: entityName.toLowerCase(),
                navNumber: navNumber,
            });
            fs.appendFileSync(genDir + '/dataTempRouter.cs', '\n' + contents + "\n");

        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.Router)
        });
        // event is emitted after each line
        let isImport = true;
        fs.writeFileSync(genDir + '/routertemp.js', '');
        rl.on('line', function(line) {
            const routerStart = '//========Import Menu Start========//';
            const routerEnd = '//========Import Menu End========//';
            if (line.includes(routerStart)) {
                fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
                isImport = false;
            }
            if (line.includes(routerEnd)) {
                var dataImport = fs.readFileSync(genDir + '/dataTempRouter.cs', '');
                fs.appendFileSync(genDir + '/routertemp.js', dataImport);
                fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
                isImport = true;
            } else if (isImport) {
                fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/routertemp.js', dir.Router);
            fs.unlinkSync(genDir + '/routertemp.js');
            fs.unlinkSync(genDir + '/dataTempRouter.cs');
            console.log(`import router ${entityName} complete !!!`);
        }, 1000);

    });
program.parse(process.argv);