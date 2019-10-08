const program = require('commander');
const fs = require('fs');
const Handlebars = require('handlebars');
const dir = require('../dir');
const config = require('config');
const allEntity = require('../Core/AllEntities');

String.prototype.capitalize = function () {
	return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};
const genDir = __dirname.replace('Command', '')

program.command('create')
	.action(() => {
		allEntity.forEach(entityName => {
			let isDateTime = false;
			const fileName = `${entityName.capitalize()}.cs`;
			const entityTemplate = [];
			fs.readFile(genDir + 'Template/Entity.html', function (err, data) {
				const template = Handlebars.compile(data.toString());
				const entityFormat = require(genDir + `/Database/${entityName}`);
				for (var key in entityFormat) {
					if (entityFormat[key].includes('DateTime')) {
						isDateTime = true;
					}
					entityTemplate.push({ 'field': key, 'type': entityFormat[key] });
				}
				var contents = template({
					entityName: entityName.capitalize(),
					EntityFields: entityTemplate,
					isDateTime: isDateTime
				});
				fs.writeFile(dir.Entity + fileName, contents, err => {
					if (err) {
						return console.error(`Autsch! Failed to store template: ${err.message}.`);
					}
					console.log(`Saved create file ${dir.Entity + fileName}`);
				});
			});
		});

	});
program.parse(process.argv);
