const program = require('commander');
const fs = require('fs');
const dir = require('../dir');
const config = require('config');
const projectName = config.get('ProjectName');
const allEntity = require('../Core/AllEntities');
const _ = require('underscore');

String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};


const genDir = __dirname.replace('Command', '')

program.command('create')
    .action(() => {
        allEntity.forEach(entityName => {
            let isDateTime = false;
            const fileName = `Shared.cshtml`;
            const entityTemplate = [];
            const entityFormat = require(genDir + `/Database/${entityName}`);
            fs.readFile(genDir + 'Template/View/Share.html', function (err, data) {
                for (var key in entityFormat) {
                    if (entityFormat[key].includes('DateTime')) {
                        isDateTime = true;
                    }
                    if (!entityFormat[key].includes('virtual')) {
                        entityTemplate.push({ 'field': key, 'type': entityFormat[key] });
                    }
                }
                var contents = _.template(data.toString())({
                    entityName: entityName.capitalize(),
                    EntityFields: entityTemplate,
                    projectName: projectName,
                    isDateTime: isDateTime
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
