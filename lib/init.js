const fs = require( 'fs' );

exports.command = 'init <name>';

exports.aliases = [ 'new' ];

exports.describe = 'initialize a Project';

exports.builder = {
    type: {
        alias: 't',
        describe: 'set project type'
    }
};

exports.handler = function (argv) {
    let project_path = `./${argv.name}`;

    if ( fs.existsSync( project_path ) ) {
        console.error( `'${project_path}' already exists` );

        return ;

    } else {
        console.log( '  mkdir %s', project_path );
        //fs.mkdirSync( project_path );
    }
};
