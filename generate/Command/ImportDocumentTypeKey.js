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
        fs.writeFileSync(genDir + '/documentTypeKeytemp.cs', '');
        let documentTypeKey = 3;
        allEntity.forEach(entityName => {
            documentTypeKey++;
            let rl = readline.createInterface({
                input: fs.createReadStream(dir.DocumentTypeKey)
            });
            // event is emitted after each line
            let isImport = true;
            rl.on('line', function (line) {

                const documentTypeKeyStart = '//========Import DocumnetTypeKey Start========//';
                const documentTypeKeyEnd = '//========Import DocumnetTypeKey End========//';
                if (line.includes(documentTypeKeyStart)) {
                    fs.appendFileSync(genDir + '/documentTypeKeytemp.cs', line.toString() + "\n");
                    isImport = false;
                }
                if (line.includes(documentTypeKeyEnd)) {
                    const data = `        ${entityName.capitalize()} = ${documentTypeKey}`
                    fs.appendFileSync(genDir + '/documentTypeKeytemp.cs', '\n' + data + "\n");
                    fs.appendFileSync(genDir + '/documentTypeKeytemp.cs', line.toString() + "\n");
                    isImport = true;
                } else if (isImport) {
                    fs.appendFileSync(genDir + '/documentTypeKeytemp.cs', line.toString() + "\n");
                }
            })
            setTimeout(() => {
                fs.copyFileSync(genDir + '/documentTypeKeytemp.cs', dir.DocumentTypeKey);
                fs.unlinkSync(genDir + '/documentTypeKeytemp.cs');
                console.log(`import documentTypeKey ${entityName} complete !!!`);
            }, 1000);

        });

    });
program.parse(process.argv);
