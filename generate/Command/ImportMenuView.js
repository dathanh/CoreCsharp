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
        fs.writeFileSync(genDir + '/dataTempMenuView.cs', '');
        allEntity.forEach(entityName => {
            const data = `                objResult.CanView${entityName.capitalize()} = CheckUserRoleForDocumentType(idRole, DocumentTypeKey.${entityName.capitalize()}, OperationAction.ShowMenu);`
            fs.appendFileSync(genDir + '/dataTempMenuView.cs', '\n' + data + "\n");
        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.MenuExtraData)
        });
        // event is emitted after each line
        let isImport = true;
        rl.on('line', function(line) {
            const menuExStart = '//========Import Menu Start========//';
            const menuExEnd = '//========Import Menu End========//';
            if (line.includes(menuExStart)) {
                fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
                isImport = false;
            }
            if (line.includes(menuExEnd)) {
                var dataImport = fs.readFileSync(genDir + '/dataTempMenuView.cs', '');
                fs.appendFileSync(genDir + '/menuExtemp.cs', dataImport);
                fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
                isImport = true;
            } else if (isImport) {
                fs.appendFileSync(genDir + '/menuExtemp.cs', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/menuExtemp.cs', dir.MenuExtraData);
            fs.unlinkSync(genDir + '/menuExtemp.cs');
            fs.unlinkSync(genDir + '/dataTempMenuView.cs');
            console.log(`import menuEx ${entityName} complete !!!`);
        }, 1000);

    });
program.parse(process.argv);