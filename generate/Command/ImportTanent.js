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
                input: fs.createReadStream(dir.Tanent)
            });
            // event is emitted after each line
            let isImportDBset = true;
            let isImportDBmap = true;
            rl.on('line', function (line) {
                const dbSetImport = `public DbSet<${entityName.capitalize()}> ${entityName.capitalize()} { get; set; }`;
                const dbMapImport = `modelBuilder.ApplyConfiguration(new ${entityName.capitalize()}Map());`;
                const condDBset = line.includes('//=====Import DB Set=======//');
                const condDBmap = line.includes('//=====Import DB Map=======//');
                if (line.includes(dbSetImport)) {
                    isImportDBset = false;
                }
                if (line.includes(dbMapImport)) {
                    isImportDBmap = false;
                }
                if (condDBset || condDBmap) {
                    if (condDBset && isImportDBset) {
                        fs.appendFileSync(genDir + '/tmp.cs', '        ' + dbSetImport + "\n");
                    }
                    if (condDBmap && isImportDBmap) {
                        fs.appendFileSync(genDir + '/tmp.cs', '            ' + dbMapImport + "\n");
                    }
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                } else {
                    fs.appendFileSync(genDir + '/tmp.cs', line.toString() + "\n");
                }
            })
            setTimeout(() => {
                fs.copyFileSync(genDir + '/tmp.cs', dir.Tanent);
                fs.unlinkSync(genDir + '/tmp.cs');
                console.log(`import tanent ${entityName} complete !!!`);
            }, 200);

        });

    });
program.parse(process.argv);
