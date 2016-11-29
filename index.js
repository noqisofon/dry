const package_json = require( './package.json' );

const yargs        = require( 'yargs' );

yargs
    .command( require( './lib/init' ) )
    .help()
    .version( () => `${package_json.name} ${package_json.version}` )
    .argv;
