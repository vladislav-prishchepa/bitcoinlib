// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Requests.AddNode;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Requests.SignRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.RPC.Specifications;
using Newtonsoft.Json.Linq;

namespace BitcoinLib.Services
{
    //   Implementation of API calls list, as found at: https://en.bitcoin.it/wiki/Original_Bitcoin_client/API_Calls_list (note: this list is often out-of-date so call "help" in your bitcoin-cli to get the latest signatures)
    public partial class CoinService
    {
        public Task<string> AddMultiSigAddressAsync(int nRquired, List<string> publicKeys, string account, CancellationToken cancellationToken)
        {
            return account != null
                ? _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.addmultisigaddress, cancellationToken, nRquired, publicKeys, account)
                : _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.addmultisigaddress, cancellationToken, nRquired, publicKeys);
        }

        public Task AddNodeAsync(string node, NodeAction action, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.addnode, cancellationToken, node, action.ToString());
        }

        public Task<string> AddWitnessAddressAsync(string address, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.addwitnessaddress, cancellationToken, address);
        }

        public Task BackupWalletAsync(string destination, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.backupwallet, cancellationToken, destination);
        }

        public Task<CreateMultiSigResponse> CreateMultiSigAsync(int nRquired, List<string> publicKeys, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<CreateMultiSigResponse>(RpcMethods.createmultisig, cancellationToken, nRquired, publicKeys);
        }

        public Task<string> CreateRawTransactionAsync(CreateRawTransactionRequest rawTransaction, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.createrawtransaction, cancellationToken, rawTransaction.Inputs, rawTransaction.Outputs);
        }

        public Task<DecodeRawTransactionResponse> DecodeRawTransactionAsync(string rawTransactionHexString, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<DecodeRawTransactionResponse>(RpcMethods.decoderawtransaction, cancellationToken, rawTransactionHexString);
        }

        public Task<DecodeScriptResponse> DecodeScriptAsync(string hexString, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<DecodeScriptResponse>(RpcMethods.decodescript, cancellationToken, hexString);
        }

        public Task<string> DumpPrivKeyAsync(string bitcoinAddress, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.dumpprivkey, cancellationToken, bitcoinAddress);
        }

        public Task DumpWalletAsync(string filename, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.dumpwallet, cancellationToken, filename);
        }

        public Task<decimal> EstimateFeeAsync(ushort nBlocks, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.estimatefee, cancellationToken, nBlocks);
        }

        public Task<EstimateSmartFeeResponse> EstimateSmartFeeAsync(ushort nBlocks, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<EstimateSmartFeeResponse>(RpcMethods.estimatesmartfee, cancellationToken, nBlocks);
        }

        public Task<decimal> EstimatePriorityAsync(ushort nBlocks, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.estimatepriority, cancellationToken, nBlocks);
        }

        public Task<string> GetAccountAsync(string bitcoinAddress, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getaccount, cancellationToken, bitcoinAddress);
        }

        public Task<string> GetAccountAddressAsync(string account, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getaccountaddress, cancellationToken, account);
        }

        public Task<GetAddedNodeInfoResponse> GetAddedNodeInfoAsync(string dns, string node, CancellationToken cancellationToken)
        {
            return string.IsNullOrWhiteSpace(node)
                ? _asyncRpcConnector.MakeRequestAsync<GetAddedNodeInfoResponse>(RpcMethods.getaddednodeinfo, cancellationToken, dns)
                : _asyncRpcConnector.MakeRequestAsync<GetAddedNodeInfoResponse>(RpcMethods.getaddednodeinfo, cancellationToken, dns, node);
        }

        public Task<List<string>> GetAddressesByAccountAsync(string account, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<string>>(RpcMethods.getaddressesbyaccount, cancellationToken, account);
        }

        public Task<Dictionary<string, GetAddressesByLabelResponse>> GetAddressesByLabelAsync(string label, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<Dictionary<string, GetAddressesByLabelResponse>>(RpcMethods.getaddressesbylabel, cancellationToken, label);
        }

        public Task<GetAddressInfoResponse> GetAddressInfoAsync(string bitcoinAddress, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetAddressInfoResponse>(RpcMethods.getaddressinfo, cancellationToken, bitcoinAddress);
        }

        public Task<decimal> GetBalanceAsync(string account, int minConf, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getbalance, cancellationToken, (string.IsNullOrWhiteSpace(account) ? "*" : account), minConf)
                : _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getbalance, cancellationToken, (string.IsNullOrWhiteSpace(account) ? "*" : account), minConf, includeWatchonly);
        }

        public Task<string> GetBestBlockHashAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getbestblockhash, cancellationToken);
        }

        public Task<GetBlockResponse> GetBlockAsync(string hash, bool verbose, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetBlockResponse>(RpcMethods.getblock, cancellationToken, hash, verbose);
        }

        public Task<GetBlockchainInfoResponse> GetBlockchainInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetBlockchainInfoResponse>(RpcMethods.getblockchaininfo, cancellationToken);
        }

        public Task<uint> GetBlockCountAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<uint>(RpcMethods.getblockcount, cancellationToken);
        }

        public Task<string> GetBlockHashAsync(long index, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getblockhash, cancellationToken, index);
        }

        public Task<GetBlockTemplateResponse> GetBlockTemplateAsync(params object[] parameters)
        {
            return GetBlockTemplateAsync(CancellationToken.None, parameters);
        }

        public Task<GetBlockTemplateResponse> GetBlockTemplateAsync(CancellationToken cancellationToken, params object[] parameters)
        {
            return parameters == null
                ? _asyncRpcConnector.MakeRequestAsync<GetBlockTemplateResponse>(RpcMethods.getblocktemplate, cancellationToken)
                : _asyncRpcConnector.MakeRequestAsync<GetBlockTemplateResponse>(RpcMethods.getblocktemplate, cancellationToken, parameters);
        }

        public Task<List<GetChainTipsResponse>> GetChainTipsAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<GetChainTipsResponse>>(RpcMethods.getchaintips, cancellationToken);
        }

        public Task<int> GetConnectionCountAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<int>(RpcMethods.getconnectioncount, cancellationToken);
        }

        public Task<double> GetDifficultyAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<double>(RpcMethods.getdifficulty, cancellationToken);
        }

        public Task<bool> GetGenerateAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.getgenerate, cancellationToken);
        }

        [Obsolete("Please use calls: GetWalletInfoAsync(), GetBlockchainInfoAsync() and GetNetworkInfo() instead")]
        public Task<GetInfoResponse> GetInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetInfoResponse>(RpcMethods.getinfo, cancellationToken);
        }

        public Task<GetMemPoolInfoResponse> GetMemPoolInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetMemPoolInfoResponse>(RpcMethods.getmempoolinfo, cancellationToken);
        }

        public Task<GetMiningInfoResponse> GetMiningInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetMiningInfoResponse>(RpcMethods.getmininginfo, cancellationToken);
        }

        public Task<GetNetTotalsResponse> GetNetTotalsAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetNetTotalsResponse>(RpcMethods.getnettotals, cancellationToken);
        }

        public Task<ulong> GetNetworkHashPsAsync(uint blocks, long height, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<ulong>(RpcMethods.getnetworkhashps, cancellationToken);
        }

        public Task<GetNetworkInfoResponse> GetNetworkInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetNetworkInfoResponse>(RpcMethods.getnetworkinfo, cancellationToken);
        }

        public Task<string> GetNewAddressAsync(string account, CancellationToken cancellationToken)
        {
            return string.IsNullOrWhiteSpace(account)
                ? _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getnewaddress, cancellationToken)
                : _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getnewaddress, cancellationToken, account);
        }

        public Task<List<GetPeerInfoResponse>> GetPeerInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<GetPeerInfoResponse>>(RpcMethods.getpeerinfo, cancellationToken);
        }

        public async Task<GetRawMemPoolResponse> GetRawMemPoolAsync(bool verbose, CancellationToken cancellationToken)
        {
            var getRawMemPoolResponse = new GetRawMemPoolResponse
            {
                IsVerbose = verbose
            };

            var rpcResponse = await _asyncRpcConnector.MakeRequestAsync<object>(RpcMethods.getrawmempool, cancellationToken, verbose).ConfigureAwait(false);

            if (!verbose)
            {
                var rpcResponseAsArray = (JArray) rpcResponse;

                foreach (string txId in rpcResponseAsArray)
                {
                    getRawMemPoolResponse.TxIds.Add(txId);
                }

                return getRawMemPoolResponse;
            }

            IList<KeyValuePair<string, JToken>> rpcResponseAsKvp = (new EnumerableQuery<KeyValuePair<string, JToken>>(((JObject) (rpcResponse)))).ToList();
            IList<JToken> children = JObject.Parse(rpcResponse.ToString()).Children().ToList();

            for (var i = 0; i < children.Count(); i++)
            {
                var getRawMemPoolVerboseResponse = new GetRawMemPoolVerboseResponse
                {
                    TxId = rpcResponseAsKvp[i].Key
                };

                getRawMemPoolResponse.TxIds.Add(getRawMemPoolVerboseResponse.TxId);

                foreach (var property in children[i].SelectMany(grandChild => grandChild.OfType<JProperty>()))
                {
                    switch (property.Name)
                    {
                        case "currentpriority":

                            double currentPriority;

                            if (double.TryParse(property.Value.ToString(), out currentPriority))
                            {
                                getRawMemPoolVerboseResponse.CurrentPriority = currentPriority;
                            }

                            break;

                        case "depends":

                            foreach (var jToken in property.Value)
                            {
                                getRawMemPoolVerboseResponse.Depends.Add(jToken.Value<string>());
                            }

                            break;

                        case "fee":

                            decimal fee;

                            if (decimal.TryParse(property.Value.ToString(), out fee))
                            {
                                getRawMemPoolVerboseResponse.Fee = fee;
                            }

                            break;

                        case "height":

                            int height;

                            if (int.TryParse(property.Value.ToString(), out height))
                            {
                                getRawMemPoolVerboseResponse.Height = height;
                            }

                            break;

                        case "size":

                            int size;

                            if (int.TryParse(property.Value.ToString(), out size))
                            {
                                getRawMemPoolVerboseResponse.Size = size;
                            }

                            break;

                        case "startingpriority":

                            double startingPriority;

                            if (double.TryParse(property.Value.ToString(), out startingPriority))
                            {
                                getRawMemPoolVerboseResponse.StartingPriority = startingPriority;
                            }

                            break;

                        case "time":

                            int time;

                            if (int.TryParse(property.Value.ToString(), out time))
                            {
                                getRawMemPoolVerboseResponse.Time = time;
                            }

                            break;

                        default:

                            throw new Exception("Unkown property: " + property.Name + " in GetRawMemPool()");
                    }
                }
                getRawMemPoolResponse.VerboseResponses.Add(getRawMemPoolVerboseResponse);
            }
            return getRawMemPoolResponse;
        }

        public Task<string> GetRawChangeAddressAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getrawchangeaddress, cancellationToken);
        }

        public async Task<GetRawTransactionResponse> GetRawTransactionAsync(string txId, int verbose, CancellationToken cancellationToken)
        {
            if (verbose == 0)
            {
                return new GetRawTransactionResponse
                {
                    Hex = await _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.getrawtransaction, cancellationToken, txId, verbose).ConfigureAwait(false)
                };
            }

            if (verbose == 1)
            {
                return await _asyncRpcConnector.MakeRequestAsync<GetRawTransactionResponse>(RpcMethods.getrawtransaction, cancellationToken, txId, verbose).ConfigureAwait(false);
            }

            throw new Exception("Invalid verbose value: " + verbose + " in GetRawTransaction()!");
        }

        public Task<decimal> GetReceivedByAccountAsync(string account, int minConf, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getreceivedbyaccount, cancellationToken, account, minConf);
        }

        public Task<decimal> GetReceivedByAddressAsync(string bitcoinAddress, int minConf, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getreceivedbyaddress, cancellationToken, bitcoinAddress, minConf);
        }

        public Task<decimal> GetReceivedByLabelAsync(string bitcoinAddress, int minConf, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getreceivedbylabel, cancellationToken, bitcoinAddress, minConf);
        }

        public Task<GetTransactionResponse> GetTransactionAsync(string txId, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<GetTransactionResponse>(RpcMethods.gettransaction, cancellationToken, txId)
                : _asyncRpcConnector.MakeRequestAsync<GetTransactionResponse>(RpcMethods.gettransaction, cancellationToken, txId, includeWatchonly);
        }

        public Task<GetTransactionResponse> GetTxOutAsync(string txId, int n, bool includeMemPool, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetTransactionResponse>(RpcMethods.gettxout, cancellationToken, txId, n, includeMemPool);
        }

        public Task<GetTxOutSetInfoResponse> GetTxOutSetInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetTxOutSetInfoResponse>(RpcMethods.gettxoutsetinfo, cancellationToken);
        }

        public Task<decimal> GetUnconfirmedBalanceAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<decimal>(RpcMethods.getunconfirmedbalance, cancellationToken);
        }

        public Task<GetWalletInfoResponse> GetWalletInfoAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetWalletInfoResponse>(RpcMethods.getwalletinfo, cancellationToken);
        }

        public Task<string> HelpAsync(string command, CancellationToken cancellationToken)
        {
            return string.IsNullOrWhiteSpace(command)
                ? _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.help, cancellationToken)
                : _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.help, cancellationToken, command);
        }

        public Task ImportAddressAsync(string address, string label, bool rescan, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.importaddress, cancellationToken, address, label, rescan);
        }

        public Task<string> ImportPrivKeyAsync(string privateKey, string label, bool rescan, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.importprivkey, cancellationToken, privateKey, label, rescan);
        }

        public Task ImportWalletAsync(string filename, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.importwallet, cancellationToken, filename);
        }

        public Task<string> KeyPoolRefillAsync(uint newSize, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.keypoolrefill, cancellationToken, newSize);
        }

        public Task<Dictionary<string, decimal>> ListAccountsAsync(int minConf, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<Dictionary<string, decimal>>(RpcMethods.listaccounts, cancellationToken, minConf)
                : _asyncRpcConnector.MakeRequestAsync<Dictionary<string, decimal>>(RpcMethods.listaccounts, cancellationToken, minConf, includeWatchonly);
        }

        public async Task<List<List<ListAddressGroupingsResponse>>> ListAddressGroupingsAsync(CancellationToken cancellationToken)
        {
            var unstructuredResponse = await _asyncRpcConnector.MakeRequestAsync<List<List<List<object>>>>(RpcMethods.listaddressgroupings, cancellationToken).ConfigureAwait(false);
            var structuredResponse = new List<List<ListAddressGroupingsResponse>>(unstructuredResponse.Count);

            for (var i = 0; i < unstructuredResponse.Count; i++)
            {
                for (var j = 0; j < unstructuredResponse[i].Count; j++)
                {
                    if (unstructuredResponse[i][j].Count > 1)
                    {
                        var response = new ListAddressGroupingsResponse
                        {
                            Address = unstructuredResponse[i][j][0].ToString()
                        };

                        decimal balance;
                        if (decimal.TryParse(unstructuredResponse[i][j][1].ToString(), out balance))
                        {
                            response.Balance = balance;
                        }

                        if (unstructuredResponse[i][j].Count > 2)
                        {
                            response.Account = unstructuredResponse[i][j][2].ToString();
                        }

                        if (structuredResponse.Count < i + 1)
                        {
                            structuredResponse.Add(new List<ListAddressGroupingsResponse>());
                        }

                        structuredResponse[i].Add(response);
                    }
                }
            }
            return structuredResponse;
        }

        public Task<List<string>> ListLabelsAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<string>>(RpcMethods.listlabels, cancellationToken);
        }

        public Task<string> ListLockUnspentAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.listlockunspent, cancellationToken);
        }

        public Task<List<ListReceivedByAccountResponse>> ListReceivedByAccountAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<List<ListReceivedByAccountResponse>>(RpcMethods.listreceivedbyaccount, cancellationToken, minConf, includeEmpty)
                : _asyncRpcConnector.MakeRequestAsync<List<ListReceivedByAccountResponse>>(RpcMethods.listreceivedbyaccount, cancellationToken, minConf, includeEmpty, includeWatchonly);
        }

        public Task<List<ListReceivedByAddressResponse>> ListReceivedByAddressAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<List<ListReceivedByAddressResponse>>(RpcMethods.listreceivedbyaddress, cancellationToken, minConf, includeEmpty)
                : _asyncRpcConnector.MakeRequestAsync<List<ListReceivedByAddressResponse>>(RpcMethods.listreceivedbyaddress, cancellationToken, minConf, includeEmpty, includeWatchonly);
        }

        public Task<List<ListReceivedByLabelResponse>> ListReceivedByLabelAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<ListReceivedByLabelResponse>>(RpcMethods.listreceivedbylabel, cancellationToken, minConf, includeEmpty, includeWatchonly);
        }

        public Task<ListSinceBlockResponse> ListSinceBlockAsync(string blockHash, int targetConfirmations, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<ListSinceBlockResponse>(RpcMethods.listsinceblock, cancellationToken, (string.IsNullOrWhiteSpace(blockHash) ? "" : blockHash), targetConfirmations)
                : _asyncRpcConnector.MakeRequestAsync<ListSinceBlockResponse>(RpcMethods.listsinceblock, cancellationToken, (string.IsNullOrWhiteSpace(blockHash) ? "" : blockHash), targetConfirmations, includeWatchonly);
        }

        public Task<List<ListTransactionsResponse>> ListTransactionsAsync(string account, int count, int from, bool? includeWatchonly, CancellationToken cancellationToken)
        {
            return includeWatchonly == null
                ? _asyncRpcConnector.MakeRequestAsync<List<ListTransactionsResponse>>(RpcMethods.listtransactions, cancellationToken, (string.IsNullOrWhiteSpace(account) ? "*" : account), count, from)
                : _asyncRpcConnector.MakeRequestAsync<List<ListTransactionsResponse>>(RpcMethods.listtransactions, cancellationToken, (string.IsNullOrWhiteSpace(account) ? "*" : account), count, from, includeWatchonly);
        }

        public Task<List<ListUnspentResponse>> ListUnspentAsync(int minConf, int maxConf, List<string> addresses, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<List<ListUnspentResponse>>(RpcMethods.listunspent, cancellationToken, minConf, maxConf, (addresses ?? new List<string>()));
        }

        public Task<bool> LockUnspentAsync(bool unlock, IList<ListUnspentResponse> listUnspentResponses, CancellationToken cancellationToken)
        {
            IList<object> transactions = new List<object>();

            foreach (var listUnspentResponse in listUnspentResponses)
            {
                transactions.Add(new
                {
                    txid = listUnspentResponse.TxId, vout = listUnspentResponse.Vout
                });
            }

            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.lockunspent, cancellationToken, unlock, transactions.ToArray());
        }

        public Task<bool> MoveAsync(string fromAccount, string toAccount, decimal amount, int minConf, string comment, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.move, cancellationToken, fromAccount, toAccount, amount, minConf, comment);
        }

        public Task PingAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.ping, cancellationToken);
        }

        public Task<bool> PrioritiseTransactionAsync(string txId, decimal priorityDelta, decimal feeDelta, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.prioritisetransaction, cancellationToken, txId, priorityDelta, feeDelta);
        }

        public Task<string> SendFromAsync(string fromAccount, string toBitcoinAddress, decimal amount, int minConf, string comment, string commentTo, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.sendfrom, cancellationToken, fromAccount, toBitcoinAddress, amount, minConf, comment, commentTo);
        }

        public Task<string> SendManyAsync(string fromAccount, Dictionary<string, decimal> toBitcoinAddress, int minConf, string comment, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.sendmany, cancellationToken, fromAccount, toBitcoinAddress, minConf, comment);
        }

        public Task<string> SendRawTransactionAsync(string rawTransactionHexString, bool? allowHighFees, CancellationToken cancellationToken)
        {
            return allowHighFees == null
                ? _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.sendrawtransaction, cancellationToken, rawTransactionHexString)
                : _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.sendrawtransaction, cancellationToken, rawTransactionHexString, allowHighFees);
        }

        public Task<string> SendToAddressAsync(string bitcoinAddress, decimal amount, string comment, string commentTo, bool subtractFeeFromAmount, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.sendtoaddress, cancellationToken, bitcoinAddress, amount, comment, commentTo, subtractFeeFromAmount);
        }

        public Task<string> SetAccountAsync(string bitcoinAddress, string account, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.setaccount, cancellationToken, bitcoinAddress, account);
        }

        public Task<string> SetLabelAsync(string bitcoinAddress, string label, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.setlabel, cancellationToken, bitcoinAddress, label);
        }

        public Task<string> SetGenerateAsync(bool generate, short generatingProcessorsLimit, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.setgenerate, cancellationToken, generate, generatingProcessorsLimit);
        }

        public Task<string> SetTxFeeAsync(decimal amount, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.settxfee, cancellationToken, amount);
        }

        public Task<string> SignMessageAsync(string bitcoinAddress, string message, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.signmessage, cancellationToken, bitcoinAddress, message);
        }

        public Task<SignRawTransactionResponse> SignRawTransactionAsync(SignRawTransactionRequest request, CancellationToken cancellationToken)
        {
            #region default values

            if (request.Inputs.Count == 0)
            {
                request.Inputs = null;
            }

            if (string.IsNullOrWhiteSpace(request.SigHashType))
            {
                request.SigHashType = SigHashType.All;
            }

            if (request.PrivateKeys.Count == 0)
            {
                request.PrivateKeys = null;
            }

            #endregion

            return _asyncRpcConnector.MakeRequestAsync<SignRawTransactionResponse>(RpcMethods.signrawtransaction, cancellationToken, request.RawTransactionHex, request.Inputs, request.PrivateKeys, request.SigHashType);
        }

        public Task<SignRawTransactionWithKeyResponse> SignRawTransactionWithKeyAsync(SignRawTransactionWithKeyRequest request, CancellationToken cancellationToken)
        {
            #region default values

            if (request.PrivateKeys.Count == 0)
            {
                request.PrivateKeys = null;
            }

            if (request.Inputs.Count == 0)
            {
                request.Inputs = null;
            }

            if (string.IsNullOrWhiteSpace(request.SigHashType))
            {
                request.SigHashType = SigHashType.All;
            }

            #endregion

            return _asyncRpcConnector.MakeRequestAsync<SignRawTransactionWithKeyResponse>(RpcMethods.signrawtransactionwithkey, cancellationToken, request.RawTransactionHex, request.PrivateKeys, request.Inputs, request.SigHashType);
        }

        public Task<SignRawTransactionWithWalletResponse> SignRawTransactionWithWalletAsync(SignRawTransactionWithWalletRequest request, CancellationToken cancellationToken)
        {
            #region default values

            if (request.Inputs.Count == 0)
            {
                request.Inputs = null;
            }

            if (string.IsNullOrWhiteSpace(request.SigHashType))
            {
                request.SigHashType = SigHashType.All;
            }

            #endregion

            return _asyncRpcConnector.MakeRequestAsync<SignRawTransactionWithWalletResponse>(RpcMethods.signrawtransactionwithwallet, cancellationToken, request.RawTransactionHex, request.Inputs, request.SigHashType);
        }

        public Task<GetFundRawTransactionResponse> GetFundRawTransactionAsync(string rawTransactionHex, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<GetFundRawTransactionResponse>(RpcMethods.fundrawtransaction, cancellationToken, rawTransactionHex);
        }

        public Task<string> StopAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.stop, cancellationToken);
        }

        public Task<string> SubmitBlockAsync(string hexData, params object[] parameters)
        {
            return SubmitBlockAsync(hexData, CancellationToken.None, parameters);
        }

        public Task<string> SubmitBlockAsync(string hexData, CancellationToken cancellationToken, params object[] parameters)
        {
            return parameters == null
                ? _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.submitblock, cancellationToken, hexData)
                : _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.submitblock, cancellationToken, hexData, parameters);
        }

        public Task<ValidateAddressResponse> ValidateAddressAsync(string bitcoinAddress, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<ValidateAddressResponse>(RpcMethods.validateaddress, cancellationToken, bitcoinAddress);
        }

        public Task<bool> VerifyChainAsync(ushort checkLevel, uint numBlocks, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.verifychain, cancellationToken, checkLevel, numBlocks);
        }

        public Task<bool> VerifyMessageAsync(string bitcoinAddress, string signature, string message, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<bool>(RpcMethods.verifymessage, cancellationToken, bitcoinAddress, signature, message);
        }

        public Task<string> WalletLockAsync(CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.walletlock, cancellationToken);
        }

        public Task<string> WalletPassphraseAsync(string passphrase, int timeoutInSeconds, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.walletpassphrase, cancellationToken, passphrase, timeoutInSeconds);
        }

        public Task<string> WalletPassphraseChangeAsync(string oldPassphrase, string newPassphrase, CancellationToken cancellationToken)
        {
            return _asyncRpcConnector.MakeRequestAsync<string>(RpcMethods.walletpassphrasechange, cancellationToken, oldPassphrase, newPassphrase);
        }
    }
}
