const program = require('commander');
const fs = require('fs');
const Handlebars = require('handlebars');
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
            const entityTemplate = [];
            fs.readFile(genDir + 'Template/Sql.html', function (err, data) {
                const entityFormat = require(genDir + `/Database/${entityName}`);
                for (var key in entityFormat) {
                    if (!entityFormat[key].includes('virtual')) {
                        entityTemplate.push({ 'field': key, 'type': entityFormat[key] });
                    }

                }
                var contents = _.template(data.toString())({
                    entityName: entityName.capitalize(),
                    EntityFields: entityTemplate,
                    projectName: projectName,
                });
                fs.appendFileSync(dir.Sql, '\n'+ contents);
                console.log('Append sql done');
            });
        });

    });
program.parse(process.argv);
