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
            const indexFile = `${entityName}Controller.js`;
            const sharefile = `shared${entityName.capitalize()}Controller.js`;
            fs.readFile(genDir + 'Template/ControllerJs/Index.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                var contents = template({
                    entityName: entityName.capitalize(),
                    entityVar: entityName.toLowerCase(),
                    projectName: projectName,
                });
                if (!fs.existsSync(dir.ControllerJs + `/${entityName.capitalize()}/`)) {
                    fs.mkdirSync(dir.ControllerJs + `/${entityName.capitalize()}/`);
                }
                fs.writeFile(dir.ControllerJs + `/${entityName.capitalize()}/` + indexFile, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.ControllerJs + `/${entityName.capitalize()}/` + indexFile}`);
                });
            });
            fs.readFile(genDir + 'Template/ControllerJs/Share.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                var contents = template({
                    entityName: entityName.capitalize(),
                    entityVar: entityName.toLowerCase(),
                    projectName: projectName,
                });
                if (!fs.existsSync(dir.ControllerJs + `/${entityName.capitalize()}/`)) {
                    fs.mkdirSync(dir.ControllerJs + `/${entityName.capitalize()}/`);
                }
                fs.writeFile(dir.ControllerJs + `/${entityName.capitalize()}/` + sharefile, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.ControllerJs + `${entityName.capitalize()}/` + sharefile}`);
                });
            });
        });

    });
program.parse(process.argv);
