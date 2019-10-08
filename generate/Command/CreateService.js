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
			const fileName = `${entityName.capitalize()}Service.cs`;
			fs.readFile(genDir + 'Template/Service.html', function (err, data) {
				const template = Handlebars.compile(data.toString());			
				var contents = template({
					entityName: entityName.capitalize(),
					entityVar: entityName.toLowerCase(),
				});
				fs.writeFile(dir.Service + fileName, contents, err => {
					if (err) {
						return console.error(`Autsch! Failed to store template: ${err.message}.`);
					}
					console.log(`Saved create file ${dir.Service + fileName}`);
				});
			});
		});
	});
program.parse(process.argv);
