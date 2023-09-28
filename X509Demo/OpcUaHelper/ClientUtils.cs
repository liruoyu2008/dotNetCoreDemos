/**********************************************

// 类名：        ClientUtils

// 功能描述：    标准opcua协议工具类。详情参考OPCFoundation在Github上的代码.

// 创建人：      ruoyu.li

// 创建时间：    2022-01-05

// 描述：        用于依赖项管理

// 修改人：

// 修改时间：

// 修改描述：

//**********************************************/

using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;

namespace X509Demo.OpcUaHelper
{
    /// <summary>
    /// Defines numerous re-useable utility functions.
    /// </summary>
    public partial class ClientUtils
    {
        /// <summary>
        /// Handles an exception.
        /// </summary>
        public static void HandleException(string caption, Exception e)
        {
            // TODO:
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="nodesToBrowse">The set of browse operations to perform.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection Browse(Session session, BrowseDescriptionCollection nodesToBrowse,
            bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();
                BrowseDescriptionCollection unprocessedOperations = new BrowseDescriptionCollection();

                while (nodesToBrowse.Count > 0)
                {
                    // start the browse operation.
                    BrowseResultCollection results = null;
                    DiagnosticInfoCollection diagnosticInfos = null;

                    session.Browse(
                        null,
                        null,
                        0,
                        nodesToBrowse,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, nodesToBrowse);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                    ByteStringCollection continuationPoints = new ByteStringCollection();

                    for (int ii = 0; ii < nodesToBrowse.Count; ii++)
                    {
                        // check for error.
                        if (StatusCode.IsBad(results[ii].StatusCode))
                        {
                            // this error indicates that the server does not have enough simultaneously active 
                            // continuation points. This request will need to be resent after the other operations
                            // have been completed and their continuation points released.
                            if (results[ii].StatusCode == StatusCodes.BadNoContinuationPoints)
                            {
                                unprocessedOperations.Add(nodesToBrowse[ii]);
                            }

                            continue;
                        }

                        // check if all references have been fetched.
                        if (results[ii].References.Count == 0)
                        {
                            continue;
                        }

                        // save results.
                        references.AddRange(results[ii].References);

                        // check for continuation point.
                        if (results[ii].ContinuationPoint != null)
                        {
                            continuationPoints.Add(results[ii].ContinuationPoint);
                        }
                    }

                    // process continuation points.
                    ByteStringCollection revisedContiuationPoints = new ByteStringCollection();

                    while (continuationPoints.Count > 0)
                    {
                        // continue browse operation.
                        session.BrowseNext(
                            null,
                            true,
                            continuationPoints,
                            out results,
                            out diagnosticInfos);

                        ClientBase.ValidateResponse(results, continuationPoints);
                        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);

                        for (int ii = 0; ii < continuationPoints.Count; ii++)
                        {
                            // check for error.
                            if (StatusCode.IsBad(results[ii].StatusCode))
                            {
                                continue;
                            }

                            // check if all references have been fetched.
                            if (results[ii].References.Count == 0)
                            {
                                continue;
                            }

                            // save results.
                            references.AddRange(results[ii].References);

                            // check for continuation point.
                            if (results[ii].ContinuationPoint != null)
                            {
                                revisedContiuationPoints.Add(results[ii].ContinuationPoint);
                            }
                        }

                        // check if browsing must continue;
                        revisedContiuationPoints = continuationPoints;
                    }

                    // check if unprocessed results exist.
                    nodesToBrowse = unprocessedOperations;
                }

                // return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="nodeToBrowse">The NodeId for the starting node.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection Browse(Session session, BrowseDescription nodeToBrowse,
            bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

                // construct browse request.
                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                nodesToBrowse.Add(nodeToBrowse);

                // start the browse operation.
                BrowseResultCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                session.Browse(
                    null,
                    null,
                    0,
                    nodesToBrowse,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToBrowse);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                do
                {
                    // check for error.
                    if (StatusCode.IsBad(results[0].StatusCode))
                    {
                        throw new ServiceResultException(results[0].StatusCode);
                    }

                    // process results.
                    for (int ii = 0; ii < results[0].References.Count; ii++)
                    {
                        references.Add(results[0].References[ii]);
                    }

                    // check if all references have been fetched.
                    if (results[0].References.Count == 0 || results[0].ContinuationPoint == null)
                    {
                        break;
                    }

                    // continue browse operation.
                    ByteStringCollection continuationPoints = new ByteStringCollection();
                    continuationPoints.Add(results[0].ContinuationPoint);

                    session.BrowseNext(
                        null,
                        false,
                        continuationPoints,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, continuationPoints);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);
                }
                while (true);

                //return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Browses the address space and returns all of the supertypes of the specified type node.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="typeId">The NodeId for a type node in the address space.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection BrowseSuperTypes(Session session, NodeId typeId,
            bool throwOnError)
        {
            ReferenceDescriptionCollection supertypes = new ReferenceDescriptionCollection();

            try
            {
                // find all of the children of the field.
                BrowseDescription nodeToBrowse = new BrowseDescription();

                nodeToBrowse.NodeId = typeId;
                nodeToBrowse.BrowseDirection = BrowseDirection.Inverse;
                nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.HasSubtype;
                nodeToBrowse.IncludeSubtypes = false; // more efficient to use IncludeSubtypes=False when possible.
                nodeToBrowse.NodeClassMask = 0; // the HasSubtype reference already restricts the targets to Types. 
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

                ReferenceDescriptionCollection references = Browse(session, nodeToBrowse, throwOnError);

                while (references != null && references.Count > 0)
                {
                    // should never be more than one supertype.
                    supertypes.Add(references[0]);

                    // only follow references within this server.
                    if (references[0].NodeId.IsAbsolute)
                    {
                        break;
                    }

                    // get the references for the next level up.
                    nodeToBrowse.NodeId = (NodeId)references[0].NodeId;
                    references = Browse(session, nodeToBrowse, throwOnError);
                }

                // return complete list.
                return supertypes;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the node ids for a set of relative paths.
        /// </summary>
        /// <param name="session">An open session with the server to use.</param>
        /// <param name="startNodeId">The starting node for the relative paths.</param>
        /// <param name="namespacesUris">The namespace URIs referenced by the relative paths.</param>
        /// <param name="relativePaths">The relative paths.</param>
        /// <returns>A collection of local nodes.</returns>
        public static List<NodeId> TranslateBrowsePaths(Session session, NodeId startNodeId, NamespaceTable namespacesUris,
            params string[] relativePaths)
        {
            // build the list of browse paths to follow by parsing the relative paths.
            BrowsePathCollection browsePaths = new BrowsePathCollection();

            if (relativePaths != null)
            {
                for (int ii = 0; ii < relativePaths.Length; ii++)
                {
                    BrowsePath browsePath = new BrowsePath();

                    // The relative paths used indexes in the namespacesUris table. These must be 
                    // converted to indexes used by the server. An error occurs if the relative path
                    // refers to a namespaceUri that the server does not recognize.

                    // The relative paths may refer to ReferenceType by their BrowseName. The TypeTree object
                    // allows the parser to look up the server's NodeId for the ReferenceType.

                    browsePath.RelativePath = RelativePath.Parse(
                        relativePaths[ii],
                        session.TypeTree,
                        namespacesUris,
                        session.NamespaceUris);

                    browsePath.StartingNode = startNodeId;

                    browsePaths.Add(browsePath);
                }
            }

            // make the call to the server.
            BrowsePathResultCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.TranslateBrowsePathsToNodeIds(
                null,
                browsePaths,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, browsePaths);
            Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths);

            // collect the list of node ids found.
            List<NodeId> nodes = new List<NodeId>();

            for (int ii = 0; ii < results.Count; ii++)
            {
                // check if the start node actually exists.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    nodes.Add(null);
                    continue;
                }

                // an empty list is returned if no node was found.
                if (results[ii].Targets.Count == 0)
                {
                    nodes.Add(null);
                    continue;
                }

                // Multiple matches are possible, however, the node that matches the type model is the
                // one we are interested in here. The rest can be ignored.
                BrowsePathTarget target = results[ii].Targets[0];

                if (target.RemainingPathIndex != UInt32.MaxValue)
                {
                    nodes.Add(null);
                    continue;
                }

                // The targetId is an ExpandedNodeId because it could be node in another server. 
                // The ToNodeId function is used to convert a local NodeId stored in a ExpandedNodeId to a NodeId.
                nodes.Add(ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris));
            }

            // return whatever was found.
            return nodes;
        }

    }
}
