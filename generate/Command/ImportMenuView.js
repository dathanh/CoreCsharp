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
        fs.writeFileSync(genDir + '/menuExtemp.cs', '');
        allEntity.forEach(entityName => {
            let rl = readline.createInterface({
                input: fs.createReadStream(dir.MenuExtraData)
            });
            // event is emitted after each line
            let isImport = true;
            rl.on('line', function (line) {

                const menuExStart = '//========Import Menu Start========//';
                const menuExEnd = '//========Import Menu End========//';
                if (line.includes(menuExStart)) {
                    fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
                    isImport = false;
                }
                if (line.includes(menuExEnd)) {
                    const data = `                objResult.CanView${entityName.capitalize()} = CheckUserRoleForDocumentType(idRole, DocumentTypeKey.${entityName.capitalize()}, OperationAction.ShowMenu);`
                    fs.appendFileSync(genDir + '/menuExtemp.cs', '\n' + data + "\n");
                    fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
                    isImport = true;
                } else if (isImport) {
                    fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
                }
            })
            setTimeout(() => {
					fs.copyFileSync(genDir + '/menuExtemp.cs', dir.MenuExtraData);
					fs.unlinkSync(genDir + '/menuExtemp.cs');
                console.log(`import menuEx ${entityName} complete !!!`);
            }, 1000);

        });

    });
program.parse(process.argv);
