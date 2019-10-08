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
            const fileName = `${entityName.capitalize()}Map.cs`;
            const entityTemplate = [];
            fs.readFile(genDir + 'Template/EntityMap.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                const entityFormat = require(genDir + `/Database/${entityName}`);
                for (var key in entityFormat) {
                    let isRela = false;
                    if (entityFormat[key].includes('virtual')) {
                        if (!entityFormat[key].includes('ICollection')) {
                            isRela = true;
                        }
                    }
                    entityTemplate.push({
                        'field': key,
                        'isRela': isRela
                    });
                }
                var contents = template({
                    entityName: entityName.capitalize(),
                    EntityFields: entityTemplate,
                    projectName: projectName,
                });
                fs.writeFile(dir.EntityMap + fileName, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.EntityMap + fileName}`);
                });
            });
        });
    });
program.parse(process.argv);
