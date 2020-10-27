// Extra variables that live on Global that will be replaced by webpack DefinePlugin
declare var ENV: string;
declare var API_URL: string;

interface GlobalEnvironment {
  ENV;
  API_URL;
}