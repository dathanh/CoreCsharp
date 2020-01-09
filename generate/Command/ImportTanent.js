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
        fs.writeFileSync(genDir + '/dataTemp.cs', '');
        fs.writeFileSync(genDir + '/dataTemp1.cs', '');
        allEntity.forEach(entityName => {
            const dbSetImport = `public DbSet<${entityName.capitalize()}> ${entityName.capitalize()} { get; set; }`;
            const dbMapImport = `modelBuilder.ApplyConfiguration(new ${entityName.capitalize()}Map());`;
            fs.appendFileSync(genDir + '/dataTemp.cs', '\n' + dbSetImport + "\n");
            fs.appendFileSync(genDir + '/dataTemp1.cs', '\n' + dbMapImport + "\n");
        });
        fs.writeFileSync(genDir + '/tanentTemp.cs', '');

        let rl = readline.createInterface({
            input: fs.createReadStream(dir.Tanent)
        });
        // event is emitted after each line
        let isImportDBset = true;
        let isImportDBmap = true;
        rl.on('line', function(line) {
            const condDBsetStart = line.includes('//=====Start Import DB Set=======//');
            const condDBsetEnd = line.includes('//=====End Import DB Set=======//');
            const condDBmapStart = line.includes('//=====Start Import DB Map=======//');
            const condDBmapEnd = line.includes('//=====End Import DB Map=======//');

            if (condDBsetStart) {
                fs.appendFileSync(genDir + '/tanentTemp.cs', line.toString() + "\n");
                isImportDBset = false;
            }
            if (condDBmapStart) {
                fs.appendFileSync(genDir + '/tanentTemp.cs', line.toString() + "\n");
                isImportDBmap = false;
            }
            if (condDBsetEnd || condDBmapEnd) {
                if (condDBsetEnd && !isImportDBset) {
                    var dbSetImport = fs.readFileSync(genDir + '/dataTemp.cs', '');
                    fs.appendFileSync(genDir + '/tanentTemp.cs', '        ' + dbSetImport + "\n");
                    isImportDBset = true;
                }
                if (condDBmapEnd && !isImportDBmap) {
                    var dbMapImport = fs.readFileSync(genDir + '/dataTemp1.cs', '');
                    fs.appendFileSync(genDir + '/tanentTemp.cs', '        ' + dbMapImport + "\n");
                    isImportDBmap = true;
                }
                fs.appendFileSync(genDir + '/tanentTemp.cs', line.toString() + "\n");
            } else if (isImportDBset && isImportDBmap) {
                fs.appendFileSync(genDir + '/tanentTemp.cs', line.toString() + "\n");
            }
        })
        setTimeout(() => {
            fs.copyFileSync(genDir + '/tanentTemp.cs', dir.Tanent);
            fs.unlinkSync(genDir + '/tanentTemp.cs');
            fs.unlinkSync(genDir + '/dataTemp.cs');
            fs.unlinkSync(genDir + '/dataTemp1.cs');
            console.log(`import tanent ${entityName} complete !!!`);
        }, 1000);
    });
program.parse(process.argv);