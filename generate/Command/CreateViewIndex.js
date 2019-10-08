const program = require('commander');
const fs = require('fs');
const Handlebars = require('handlebars');
const dir = require('../dir');
const config = require('config');
const projectName = config.get('ProjectName');
const allEntity = require('../Core/AllEntities');

String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('create')
    .action(() => {
        allEntity.forEach(entityName => {
            const fileName = `Index.cshtml`;
            fs.readFile(genDir + 'Template/View/Index.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                var contents = template({
                    entityName: entityName.capitalize(),
                    projectName: projectName,
                });
                if (!fs.existsSync(dir.Views + `${entityName.capitalize()}/`)) {
                    fs.mkdirSync(dir.Views + `${entityName.capitalize()}/`);
                }
                fs.writeFile(dir.Views + `${entityName.capitalize()}/` + fileName, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.Views + `${entityName.capitalize()}/` + fileName}`);
                });
            });
        });

    });
program.parse(process.argv);
