const program = require('commander');
const fs = require('fs');
const Handlebars = require('handlebars');
const dir = require('../dir');
const allEntity = require('../Core/AllEntities');

String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('create')
    .action(() => {
        allEntity.forEach(entityName => {
            const fileName = `${entityName.capitalize()}GridVo.cs`;
            const entityTemplate = [];
            var isDateTimeFlag = false;
            fs.readFile(genDir + 'Template/GridVo.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                const entityFormat = require(genDir + `/Database/${entityName}`);
                for (var key in entityFormat) {
                    if (entityFormat[key].includes('virtual')) {
                        continue;
                    }
                    let isDateTime = false;
                    if (entityFormat[key].includes('DateTime')) {
                        isDateTime = true;
                        isDateTimeFlag = true;
                    }
                    entityTemplate.push({
                        field: key,
                        type: entityFormat[key],
                        isDateTime: isDateTime
                    });
                }
                console.log(isDateTimeFlag);
                var contents = template({
                    entityName: entityName.capitalize(),
                    EntityFields: entityTemplate,
                    isDateTime: isDateTimeFlag
                });
                fs.writeFile(dir.GridVo + fileName, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.GridVo + fileName}`);
                });
            });
        });

    });
program.parse(process.argv);
