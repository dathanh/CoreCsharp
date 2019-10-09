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
            const fileName = `${entityName.capitalize()}Controller.cs`;
            const entityTemplate = [];
            fs.readFile(genDir + 'Template/ControllerCs.html', function (err, data) {
                const template = Handlebars.compile(data.toString());
                const entityFormat = require(genDir + `/Database/${entityName}`);
                let isAddLookup = false;
                let fieldLoopKup = '';
                for (var key in entityFormat) {
                    if (entityFormat[key].includes('string') || entityFormat[key].includes('bool')) {
                        entityTemplate.push({ 'field': key, 'type': entityFormat[key] });
                    }
                    if (key.includes('Name')) {
                        entityTemplate.push({ 'field': key, 'type': entityFormat[key] });
                        isAddLookup = true;
                        fieldLoopKup = key;
                    }

                }
                var contents = template({
                    entityName: entityName.capitalize(),
                    EntityFields: entityTemplate,
                    projectName: projectName,
                    isAddLookup: isAddLookup,
                    fieldLoopKup: fieldLoopKup,
                });

                fs.writeFile(dir.Controller + fileName, contents, err => {
                    if (err) {
                        return console.error(`Autsch! Failed to store template: ${err.message}.`);
                    }
                    console.log(`Saved create file ${dir.Controller + fileName}`);
                });
            });
        });

    });
program.parse(process.argv);
