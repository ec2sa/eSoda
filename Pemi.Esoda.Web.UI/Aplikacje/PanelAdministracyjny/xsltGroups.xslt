<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes" encoding="utf-8"/>


	<!-- Replace root node name Menus with MenuItems
       and call MenuListing for its children-->
	<xsl:template match="/Groups">
		<Groups>
			<xsl:call-template name="GroupListing" />
		</Groups>
	</xsl:template>

	<!-- Allow for recursive child nodeprocessing -->
	<xsl:template name="GroupListing">
		<xsl:apply-templates select="Group" />
		</xsl:template>

		<xsl:template match="Group">
			<Group>
				<!-- Convert Menu child elements to MenuItem attributes -->
				<xsl:attribute name="Text">
					<xsl:value-of select="nazwa"/>
				</xsl:attribute>
					
				<xsl:attribute name="Value">
					<xsl:value-of select="id"/>
				</xsl:attribute>
				
							<!-- Recursively call MenuListing forchild menu nodes -->
				<xsl:if test="count(Group) >0">
					<xsl:call-template name="GroupListing" />
				</xsl:if>
						
			</Group>
		</xsl:template>
	</xsl:stylesheet>