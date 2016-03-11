# splibrarytree
A web part to display the contents of a document library in a tree view.

This code is based on the project for SharePoint 2007 found here:

http://www.codeproject.com/Articles/37530/Document-Library-Tree-View-Web-Part-for-SharePoint

and is adapted for SharePoint 2013.  It is a simple web part which will display the contents of a specified document library in a tree view, with file count / size totals displayed as a tooltip on mouseover.


### Use and Options

The project packages to a Farm Solution with a Site scoped feature that deploys the web part.  Once the web part has been deployed to your site it will be available to add to any web part or wiki page.  Once added to a page, edit the web part to configure it.  The following options are exposed in the web part editor:

**Name:** Specify the document library to show in the web part by using its display name.

**Initial expansion depth:** Specify the level the tree view should be initially expanded to.

**Show lines:** Choose whether or not to show lines connecting the leaves.


### Differences from the original

The original 2007 web part used some sort of JavaScript picker to select the document library to be shown in the tree view.  I have removed the picker, now you just specify the display name of the library in the web part editor to select the library.

The original also had a hard-coded HTML title, which I removed.  You can set a title for the web part by using SharePoint's native web part chrome options.

