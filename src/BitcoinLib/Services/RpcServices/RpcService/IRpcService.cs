// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Requests.AddNode;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Requests.SignRawTransaction;
using BitcoinLib.Responses;

namespace BitcoinLib.Services.RpcServices.RpcService
{
    public interface IRpcService
    {
        #region Blockchain

        Task<string> GetBestBlockHashAsync(CancellationToken cancellationToken);
        Task<GetBlockResponse> GetBlockAsync(string hash, bool verbose, CancellationToken cancellationToken);
        Task<GetBlockchainInfoResponse> GetBlockchainInfoAsync(CancellationToken cancellationToken);
        Task<uint> GetBlockCountAsync(CancellationToken cancellationToken);
        Task<string> GetBlockHashAsync(long index, CancellationToken cancellationToken);
        //  getblockheader
        //  getchaintips
        Task<double> GetDifficultyAsync(CancellationToken cancellationToken);
        Task<List<GetChainTipsResponse>> GetChainTipsAsync(CancellationToken cancellationToken);
        Task<GetMemPoolInfoResponse> GetMemPoolInfoAsync(CancellationToken cancellationToken);
        Task<GetRawMemPoolResponse> GetRawMemPoolAsync(bool verbose, CancellationToken cancellationToken);
        Task<GetTransactionResponse> GetTxOutAsync(string txId, int n, bool includeMemPool, CancellationToken cancellationToken);
        //  gettxoutproof["txid",...] ( blockhash )
        Task<GetTxOutSetInfoResponse> GetTxOutSetInfoAsync(CancellationToken cancellationToken);
        Task<bool> VerifyChainAsync(ushort checkLevel, uint numBlocks, CancellationToken cancellationToken); //  Note: numBlocks: 0 => ALL

        #endregion

        #region Control

        Task<GetInfoResponse> GetInfoAsync(CancellationToken cancellationToken);
        Task<string> HelpAsync(string command, CancellationToken cancellationToken);
        Task<string> StopAsync(CancellationToken cancellationToken);

        #endregion

        #region Generating

        //  generate numblocks
        Task<bool> GetGenerateAsync(CancellationToken cancellationToken);
        Task<string> SetGenerateAsync(bool generate, short generatingProcessorsLimit, CancellationToken cancellationToken);

        #endregion

        #region Mining

        Task<GetBlockTemplateResponse> GetBlockTemplateAsync(CancellationToken cancellationToken, params object[] parameters);
        Task<GetMiningInfoResponse> GetMiningInfoAsync(CancellationToken cancellationToken);
        Task<ulong> GetNetworkHashPsAsync(uint blocks, long height, CancellationToken cancellationToken);
        Task<bool> PrioritiseTransactionAsync(string txId, decimal priorityDelta, decimal feeDelta, CancellationToken cancellationToken);
        Task<string> SubmitBlockAsync(string hexData, CancellationToken cancellationToken, params object[] parameters);

        #endregion

        #region Network

        Task AddNodeAsync(string node, NodeAction action, CancellationToken cancellationToken);
        //  clearbanned
        //  disconnectnode
        Task<GetAddedNodeInfoResponse> GetAddedNodeInfoAsync(string dns, string node, CancellationToken cancellationToken);
        Task<int> GetConnectionCountAsync(CancellationToken cancellationToken);
        Task<GetNetTotalsResponse> GetNetTotalsAsync(CancellationToken cancellationToken);
        Task<GetNetworkInfoResponse> GetNetworkInfoAsync(CancellationToken cancellationToken);
        Task<List<GetPeerInfoResponse>> GetPeerInfoAsync(CancellationToken cancellationToken);
        //  listbanned
        Task PingAsync(CancellationToken cancellationToken);
        //  setban

        #endregion

        #region Rawtransactions

        Task<string> CreateRawTransactionAsync(CreateRawTransactionRequest rawTransaction, CancellationToken cancellationToken);
        Task<DecodeRawTransactionResponse> DecodeRawTransactionAsync(string rawTransactionHexString, CancellationToken cancellationToken);
        Task<DecodeScriptResponse> DecodeScriptAsync(string hexString, CancellationToken cancellationToken);
        //  fundrawtransaction
        Task<GetRawTransactionResponse> GetRawTransactionAsync(string txId, int verbose, CancellationToken cancellationToken);
        Task<string> SendRawTransactionAsync(string rawTransactionHexString, bool? allowHighFees, CancellationToken cancellationToken);
        Task<SignRawTransactionResponse> SignRawTransactionAsync(SignRawTransactionRequest signRawTransactionRequest, CancellationToken cancellationToken);
        Task<SignRawTransactionWithKeyResponse> SignRawTransactionWithKeyAsync(SignRawTransactionWithKeyRequest signRawTransactionWithKeyRequest, CancellationToken cancellationToken);
        Task<SignRawTransactionWithWalletResponse> SignRawTransactionWithWalletAsync(SignRawTransactionWithWalletRequest signRawTransactionWithWalletRequest, CancellationToken cancellationToken);
        Task<GetFundRawTransactionResponse> GetFundRawTransactionAsync(string rawTransactionHex, CancellationToken cancellationToken);

        #endregion

        #region Util

        Task<CreateMultiSigResponse> CreateMultiSigAsync(int nRquired, List<string> publicKeys, CancellationToken cancellationToken);
        Task<decimal> EstimateFeeAsync(ushort nBlocks, CancellationToken cancellationToken);
        Task<EstimateSmartFeeResponse> EstimateSmartFeeAsync(ushort nBlocks, CancellationToken cancellationToken);
        Task<decimal> EstimatePriorityAsync(ushort nBlocks, CancellationToken cancellationToken);
        //  estimatesmartfee
        //  estimatesmartpriority
        Task<ValidateAddressResponse> ValidateAddressAsync(string bitcoinAddress, CancellationToken cancellationToken);
        Task<bool> VerifyMessageAsync(string bitcoinAddress, string signature, string message, CancellationToken cancellationToken);

        #endregion

        #region Wallet

        //  abandontransaction
        Task<string> AddMultiSigAddressAsync(int nRquired, List<string> publicKeys, string account, CancellationToken cancellationToken);
        Task<string> AddWitnessAddressAsync(string address, CancellationToken cancellationToken);
        Task BackupWalletAsync(string destination, CancellationToken cancellationToken);
        Task<string> DumpPrivKeyAsync(string bitcoinAddress, CancellationToken cancellationToken);
        Task DumpWalletAsync(string filename, CancellationToken cancellationToken);
        Task<string> GetAccountAsync(string bitcoinAddress, CancellationToken cancellationToken);
        Task<string> GetAccountAddressAsync(string account, CancellationToken cancellationToken);
        Task<List<string>> GetAddressesByAccountAsync(string account, CancellationToken cancellationToken);
        Task<Dictionary<string, GetAddressesByLabelResponse>> GetAddressesByLabelAsync(string label, CancellationToken cancellationToken);
        Task<GetAddressInfoResponse> GetAddressInfoAsync(string bitcoinAddress, CancellationToken cancellationToken);
        Task<decimal> GetBalanceAsync(string account, int minConf, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<string> GetNewAddressAsync(string account, CancellationToken cancellationToken);
        Task<string> GetRawChangeAddressAsync(CancellationToken cancellationToken);
        Task<decimal> GetReceivedByAccountAsync(string account, int minConf, CancellationToken cancellationToken);
        Task<decimal> GetReceivedByAddressAsync(string bitcoinAddress, int minConf, CancellationToken cancellationToken);
        Task<decimal> GetReceivedByLabelAsync(string account, int minConf, CancellationToken cancellationToken);
        Task<GetTransactionResponse> GetTransactionAsync(string txId, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<decimal> GetUnconfirmedBalanceAsync(CancellationToken cancellationToken);
        Task<GetWalletInfoResponse> GetWalletInfoAsync(CancellationToken cancellationToken);
        Task ImportAddressAsync(string address, string label, bool rescan, CancellationToken cancellationToken);
        Task<string> ImportPrivKeyAsync(string privateKey, string label, bool rescan, CancellationToken cancellationToken);
        //  importpubkey
        Task ImportWalletAsync(string filename, CancellationToken cancellationToken);
        Task<string> KeyPoolRefillAsync(uint newSize, CancellationToken cancellationToken);
        Task<Dictionary<string, decimal>> ListAccountsAsync(int minConf, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<List<List<ListAddressGroupingsResponse>>> ListAddressGroupingsAsync(CancellationToken cancellationToken);
        Task<List<string>> ListLabelsAsync(CancellationToken cancellationToken);
        Task<string> ListLockUnspentAsync(CancellationToken cancellationToken);
        Task<List<ListReceivedByAccountResponse>> ListReceivedByAccountAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<List<ListReceivedByAddressResponse>> ListReceivedByAddressAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<List<ListReceivedByLabelResponse>> ListReceivedByLabelAsync(int minConf, bool includeEmpty, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<ListSinceBlockResponse> ListSinceBlockAsync(string blockHash, int targetConfirmations, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<List<ListTransactionsResponse>> ListTransactionsAsync(string account, int count, int from, bool? includeWatchonly, CancellationToken cancellationToken);
        Task<List<ListUnspentResponse>> ListUnspentAsync(int minConf, int maxConf, List<string> addresses, CancellationToken cancellationToken);
        Task<bool> LockUnspentAsync(bool unlock, IList<ListUnspentResponse> listUnspentResponses, CancellationToken cancellationToken);
        Task<bool> MoveAsync(string fromAccount, string toAccount, decimal amount, int minConf, string comment, CancellationToken cancellationToken);
        Task<string> SendFromAsync(string fromAccount, string toBitcoinAddress, decimal amount, int minConf, string comment, string commentTo, CancellationToken cancellationToken);
        Task<string> SendManyAsync(string fromAccount, Dictionary<string, decimal> toBitcoinAddress, int minConf, string comment, CancellationToken cancellationToken);
        Task<string> SendToAddressAsync(string bitcoinAddress, decimal amount, string comment, string commentTo, bool subtractFeeFromAmount, CancellationToken cancellationToken);
        Task<string> SetAccountAsync(string bitcoinAddress, string account, CancellationToken cancellationToken);
        Task<string> SetLabelAsync(string bitcoinAddress, string label, CancellationToken cancellationToken);
        Task<string> SetTxFeeAsync(decimal amount, CancellationToken cancellationToken);
        Task<string> SignMessageAsync(string bitcoinAddress, string message, CancellationToken cancellationToken);
        Task<string> WalletLockAsync(CancellationToken cancellationToken);
        Task<string> WalletPassphraseAsync(string passphrase, int timeoutInSeconds, CancellationToken cancellationToken);
        Task<string> WalletPassphraseChangeAsync(string oldPassphrase, string newPassphrase, CancellationToken cancellationToken);

        #endregion
    }
}