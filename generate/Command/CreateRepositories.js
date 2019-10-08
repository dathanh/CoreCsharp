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
			const fileName = `EntityFramework${entityName.capitalize()}Repository.cs`;
			const entityTemplate = [];
			const searchFields = [];
			fs.readFile(genDir + 'Template/Repositories.html', function (err, data) {
				const template = Handlebars.compile(data.toString());
				const entityFormat = require(genDir + `/Database/${entityName}`);
				for (var key in entityFormat) {
					if (entityFormat[key].includes('virtual')) {
						continue;
					}
					if (entityFormat[key].includes('string')) {
						searchFields.push({
							field: key,
						});
					}
					if (entityFormat[key].includes('DateTime')) {
						key += 'Value';
					}
					entityTemplate.push({
						field: key,
					});
				}
				var contents = template({
					entityName: entityName.capitalize(),
					EntityFields: entityTemplate,
					searchFields: searchFields
				});
				fs.writeFile(dir.Repositories + fileName, contents, err => {
					if (err) {
						return console.error(`Autsch! Failed to store template: ${err.message}.`);
					}
					console.log(`Saved create file ${dir.Repositories + fileName}`);
				});
			});
		});

	});
program.parse(process.argv);
