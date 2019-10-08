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
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
    if (v1 === v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});
const genDir = __dirname.replace('Command', '')

program.command('create')
    .action(() => {
        allEntity.forEach(entityName => {
            const fileName = `${entityName.capitalize()}MappingProfile.cs`;
            const entityTemplate = [];
            fs.readFile(genDir + 'Template/Mapping.html', function (err, data) {
                const template = Handlebars.compile(data.toString());

                var contents = template({
                    entityName: entityName.capitalize(),
                    projectName: projectName
                });

                // if (!fs.existsSync(genDir)) {
                //     fs.mkdirSync(genDir + 'aaa/' + fileName);
                // }
                fs.writeFile(dir.Mapping + fileName, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.Mapping + fileName}`);
                });
            });
        });

    });
program.parse(process.argv);
