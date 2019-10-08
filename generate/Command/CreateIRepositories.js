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
			const fileName = `I${entityName.capitalize()}Repository.cs`;
			fs.readFile(genDir + 'Template/IRepositories.html', function (err, data) {
				const template = Handlebars.compile(data.toString());			
				var contents = template({
					entityName: entityName.capitalize(),
				});
				fs.writeFile(dir.IRepositories + fileName, contents, err => {
					if (err) {
						return console.error(`Autsch! Failed to store template: ${err.message}.`);
					}
					console.log(`Saved create file ${dir.IRepositories + fileName}`);
				});
			});
		});
	});
program.parse(process.argv);
