const program = require('commander');
const fs = require('fs');
const readline = require('readline');
const dir = require('../dir');
const allEntity = require('../Core/AllEntities');
const Handlebars = require('handlebars');

String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('import')
    .action(() => {
        fs.writeFileSync(genDir + '/routertemp.js', '');
        let navNumber = 1703;
        allEntity.forEach(entityName => {
            navNumber++;
            let rl = readline.createInterface({
                input: fs.createReadStream(dir.Router)
            });
            // event is emitted after each line
            let isImport = true;
            rl.on('line', function (line) {
                const routerStart = '//========Import Menu Start========//';
                const routerEnd = '//========Import Menu End========//';
                if (line.includes(routerStart)) {
                    fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
                    isImport = false;
                }
                if (line.includes(routerEnd)) {
                    const data = fs.readFileSync(genDir + 'Template/Router.html');
                    const template = Handlebars.compile(data.toString());
                    var contents = template({
                        entityName: entityName.capitalize(),
                        entityVar: entityName.toLowerCase(),
                        navNumber: navNumber,
                    });
                    fs.appendFileSync(genDir + '/routertemp.js', '\n' + contents + "\n");
                    fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
                    isImport = true;
                } else if (isImport) {
                    fs.appendFileSync(genDir + '/routertemp.js', line.toString() + "\n");
                }
            })
            setTimeout(() => {
                fs.copyFileSync(genDir + '/routertemp.js', dir.Router);
                fs.unlinkSync(genDir + '/routertemp.js');
                console.log(`import router ${entityName} complete !!!`);
            }, 200);

        });

    });
program.parse(process.argv);
