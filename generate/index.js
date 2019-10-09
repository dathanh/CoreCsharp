const fs = require('fs');
const Handlebars = require('handlebars');
const exec = require('child_process').exec;
const dir = require('./dir');
const source = '<div>{{title}}</div>';

console.log(dir);
exec("node ./Command/ImportCoreModule import", function (data) {
    console.log('ImportCoreModule done');
});
exec("node ./Command/ImportTanent import", function (data) {
    console.log('ImportTanent done');
});
exec("node ./Command/ImportWebModule import", function (data) {
    console.log('ImportWebModule done');
});
exec("node ./Command/ImportControllerJs import", function (data) {
    console.log('ImportControllerJs done');
});
exec("node ./Command/ImportRouterJs import", function (data) {
    console.log('ImportRouterJs done');
});
exec("node ./Command/ImportDocumentTypeKey import", function (data) {
    console.log('ImportDocumentTypeKey done');
});
exec("node ./Command/ImportMenuView import", function (data) {
    console.log('ImportMenuView done');
});
exec("node ./Command/CreateEntity create", function (data) {
    console.log('CreateEntity done');
});
exec("node ./Command/AppendSql create", function (data) {
    console.log('AppendSql done');
});
exec("node ./Command/CreateBR create", function (data) {
    console.log('CreateBR done');
});
exec("node ./Command/CreateControllerCs create", function (data) {
    console.log('CreateControllerCs done');
});
exec("node ./Command/CreateEntityMap create", function (data) {
    console.log('CreateEntityMap done');
});
exec("node ./Command/CreateGridVo create", function (data) {
    console.log('CreateGridVo done');
});
exec("node ./Command/CreateIRepositories create", function (data) {
    console.log('CreateIRepositories done');
});
exec("node ./Command/CreateIService create", function (data) {
    console.log('CreateIService done');
});
exec("node ./Command/CreateMapping create", function (data) {
    console.log('CreateMapping done');
});
exec("node ./Command/CreateModelData create", function (data) {
    console.log('CreateModelData done');
});
exec("node ./Command/CreateModelIndex create", function (data) {
    console.log('CreateModelIndex done');
});
exec("node ./Command/CreateModelPara create", function (data) {
    console.log('CreateModelPara done');
});
exec("node ./Command/CreateModelShare create", function (data) {
    console.log('CreateModelShare done');
});
exec("node ./Command/CreateRepositories create", function (data) {
    console.log('CreateRepositories done');
});
exec("node ./Command/CreateService create", function (data) {
    console.log('CreateService done');
});
exec("node ./Command/CreateViewIndex create", function (data) {
    console.log('CreateViewIndex done');
});
exec("node ./Command/CreateViewShare create", function (data) {
    console.log('CreateViewShare done');
});
exec("node ./Command/CreateControllerJs create", function (data) {
    console.log('CreateControllerJs done');
});



