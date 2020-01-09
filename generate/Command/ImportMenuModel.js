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
        fs.writeFileSync(genDir + '/dataTempMenuModel.cs', '');
        allEntity.forEach(entityName => {
            const data = `                 public bool CanView${entityName.capitalize()} { get; set; }`
            fs.appendFileSync(genDir + '/dataTempMenuModel.cs', '\n' + data + "\n");
        });
        let rl = readline.createInterface({
            input: fs.createReadStream(dir.MenuModel)
        });
        // event is emitted after each line
        let isImport = true;
        rl.on('line', function(line) {
            const menuExStart = '//========Import Menu Start========//';
            const menuExEnd = '//========Import Menu End========//';
            if (line.includes(menuExStart)) {
                fs.appendFileSync(genDir + '/menuModelTemp.cs', line.toString() + "\n");
                isImport = false;
            }
            if (line.includes(menuExEnd)) {
                var dataImport = fs.readFileSync(genDir + '/dataTempMenuModel.cs', '');
                fs.appendFileSync(genDir + '/menuModelTemp.cs', dataImport);
                fs.appendFileSync(genDir + '/menuModelTemp.cs', line.toString() + "\n");
                isImport = true;
            } else if (isImport) {
                fs.appendFileSync(genDir + '/menuModelTemp.cs', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/menuModelTemp.cs', dir.MenuModel);
            fs.unlinkSync(genDir + '/menuModelTemp.cs');
            fs.unlinkSync(genDir + '/dataTempMenuModel.cs');
            console.log(`import menu Model View ${entityName} complete !!!`);
        }, 1000);

    });
program.parse(process.argv);